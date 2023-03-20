using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WHSIC.Data;
using WHSIC.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace WHSIC.BusinessLogic
{
    public class SermonAlgorithm
    {
        ApplicationDbContext db = new ApplicationDbContext();
        BranchBusiness bb = new BranchBusiness();
        PastorBusiness pb = new PastorBusiness();
        AlgorithmDatesBusiness ab = new AlgorithmDatesBusiness();
        AvailabililtyBusiness avb = new AvailabililtyBusiness();
        SMS sms = new SMS();
        
        public string LastMonday()
        {
            DateTime currentDate = GetNistTime();
            string f = null;
            List<DateTime> dates = new List<DateTime>();
            int year = currentDate.Year;
            int month = currentDate.Month;
            DayOfWeek day = DayOfWeek.Monday;
            System.Globalization.CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            for (int i = 1; i <= currentCulture.Calendar.GetDaysInMonth(year, month); i++)
            {
                DateTime d = new DateTime(year, month, i);
                if (d.DayOfWeek == day)
                {
                    dates.Add(d);
                }
            }
            DateTime lastM = dates.Last();
            f += lastM.ToShortDateString();
            return f;
        }


        public void CreateSermon(string Admincontact)
        {
            int m = 0;
            List<AlgorithmDatesViewModel> algoDates = ab.GetAllAlgorithmDates();
            List<StoreCount> store = db.StoreCounts.ToList();
            AlgorithmDatesViewModel ad = ab.GetAllAlgorithmDates().Find(x => x.dateID == algoDates[0].dateID);

            if (store.Count == 0)
            {
                StoreCount st = new StoreCount
                {
                    counts = 0
                };
                db.StoreCounts.Add(st);
                db.SaveChanges();
            }
            DateTime currentDate = GetNistTime();
            List<DateTime> dates = new List<DateTime>();
            int year = currentDate.Year;
            int month = currentDate.AddMonths(1).Month;
            DayOfWeek day = DayOfWeek.Sunday;
            System.Globalization.CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

            for (int i = 1; i <= currentCulture.Calendar.GetDaysInMonth(year, month); i++)
            {
                DateTime d = new DateTime(year, month, i);
                if (d.DayOfWeek == day)
                {
                    dates.Add(d);
                }
            }



            List<PastorViewModel> ListP = (from p in pb.GetAllPastors() where p.Inside==true select p).ToList();
            List<BranchViewModel> listBran = (from b in bb.GetAllBranches() select b).ToList();
            List<AvailabilityViewModel> Listnotavail = (from lna in avb.GetAllAvailability()
                                                        where lna.Startdate.Month == currentDate.AddMonths(1).Month
                                                        select lna).ToList();

            StoreCount sc = db.StoreCounts.Find(1);
            int count = sc.counts;
            for (int x = 0; x < dates.Count(); x++)
            {
                foreach (var b in listBran)
                {
                    string d = dates[x].Date.ToShortDateString();
                    var search = db.Sermons.Where(a => a.BranchID == b.BranchID && a.Date.Equals(d)).FirstOrDefault();

                    Sermon sem = new Sermon();
                    if (count == ListP.Count())
                    {
                        count = 0;
                        foreach (var l in Listnotavail)
                        {
                            if (l.pastorID == ListP[count].pastorID)
                            {
                                if (l.Startdate <= dates[x].Date && l.EndDate >= dates[x].Date)
                                {
                                    count++;
                                }
                            }
                        }

                        if (search == null)
                        {
                            if (dates[x].Date >= ad.StartDate && dates[x].Date <= ad.EndDate)
                            {
                                sem.BranchID = b.BranchID;
                                sem.Date = dates[x].ToShortDateString();
                                sem.content = "Any desirable script";
                                sem.StartTime = "09:00 am";
                                sem.EndTime = "12:00 pm";
                                sem.PastorName = ListP[count].FirstName + " " + ListP[count].Surname;
                                db.Sermons.Add(sem);
                                db.SaveChanges();
                                m++;
                                count++;
                            }
                        }
                    }
                    else
                    {

                        foreach (var l in Listnotavail)
                        {
                            if (l.pastorID == ListP[count].pastorID)
                            {
                                if (l.Startdate <= dates[x].Date && l.EndDate >= dates[x].Date)
                                {
                                    count++;
                                }
                            }
                        }

                        if (search == null)
                        {

                            if (dates[x].Date >= ad.StartDate && dates[x].Date <= ad.EndDate)
                            {
                                sem.BranchID = b.BranchID;
                                sem.Date = dates[x].ToShortDateString();
                                sem.content = "Any desirable script";
                                sem.StartTime = "09:00 am";
                                sem.EndTime = "12:00 pm";
                                sem.PastorName = ListP[count].FirstName + " " + ListP[count].Surname;
                                db.Sermons.Add(sem);
                                db.SaveChanges();
                                m++;
                                count++;
                            }
                        }

                    }
                }
            }
            sc.counts = count;
            db.SaveChanges();
            if (m > 0)
            {
                sms.sending_full_sms(Admincontact, "Hi Administrator, there is a new Sermon schedule made and your confirmation is required in order to advertise those Sermon.");
            }

        }
        public static DateTime GetNistTime()
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    dateTime = System.DateTime.Now;
                }
                else
                {
                    StreamReader stream = new StreamReader(response.GetResponseStream());
                    string html = stream.ReadToEnd();
                    string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                    double milliseconds = Convert.ToInt64(time)/1000.0;
                    dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
                }
            }
            catch (Exception c)
            {
            }
            return dateTime;
        }
    }
}
