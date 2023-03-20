using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class AdminDonationController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        DonationBusiness DB = new DonationBusiness();
        DonationTypeBusiness dt = new DonationTypeBusiness();
        DonorBusiness d = new DonorBusiness();

        public void AddIncome(decimal amt)
        {
            decimal val = 0;

            foreach (var CO in db.Incomes.ToList())
            {
                val = CO.TotIncome;
                val = val + amt;
                CO.TotIncome = val;
                db.SaveChanges();
                break;
            }
        }
        public JsonResult GetDonations(string term)
        {
            List<string> employee = DB.GetAllDonation().Where(x => x.Donor_Name.ToUpper().StartsWith(term.ToUpper()))
                .Select(y => y.Donor_Name).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetAllDonations(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = DB.GetAllDonation().OrderByDescending(x=>x.DonationID);
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult GetAllDonations(string searchTerm, int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            List<DonationViewModel> Program;

            if (string.IsNullOrEmpty(searchTerm))
            {
                Program = DB.GetAllDonation();
            }
            else
            {
                Program = DB.GetAllDonation().Where(x => x.Donor_Name.StartsWith(searchTerm)).ToList();
            }
            var list = Program;
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateDonation()
        {
            ViewBag.Donationtype = new SelectList(dt.GetAllDonationType(), "DonationtypeID", "Description");
            return View();
        }
        [HttpPost]
        public ActionResult CreateDonation(DonationViewModel model)
        {
            ViewBag.Donationtype = new SelectList(dt.GetAllDonationType(), "DonationtypeID", "Description");
            try
            {
                string email = User.Identity.Name;

                DonorViewModel dv = d.GetAllDonor().Find(x => x.Email == email);

                model.Donor_ID = dv.Donor_ID;

                if (model.Payment_Method == null)
                {
                    model.Payment_Method = "Online Payment";
                }


                model.Date = GetNistTime();
                DB.AddDonation(model);


                if (db.Incomes.ToList().Count() == 0)
                {
                    Income I = new Income
                    {
                        TotIncome = model.Amount
                    };

                    db.Incomes.Add(I);
                    db.SaveChanges();
                }
                else
                {
                    AddIncome(model.Amount);
                }


                return RedirectToAction("ValidateCommand", "Payfast");
            }
            catch (Exception n)
            {

            }
            ViewBag.Donationtype = new SelectList(dt.GetAllDonationType(), "DonationtypeID", "Description");
            return View(model);
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
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore); //No caching

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();



                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader stream = new StreamReader(response.GetResponseStream());
                    string html = stream.ReadToEnd();
                    string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                    double milliseconds = Convert.ToInt64(time) / 1000.0;
                    dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
                }
                else
                {
                    dateTime = System.DateTime.Now;
                }
            }
            catch (Exception c)
            {

            }
            return dateTime;

        }
    }
}