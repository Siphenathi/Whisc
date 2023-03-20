using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private VisitorsBusiness _vb = new VisitorsBusiness();
        private PastorBusiness pb = new PastorBusiness();
        private BranchBusiness bb = new BranchBusiness();
        private MemberBusiness mb = new MemberBusiness();
        private DonorBusiness Db = new  DonorBusiness();
        private EmployeeBusiness eb = new EmployeeBusiness();
        private NotificationBusiness nb = new NotificationBusiness();
        private SermonBusiness sb = new SermonBusiness();
        private Expense_PaidBusiness exb = new Expense_PaidBusiness();
        private ExpenseBusiness esb = new ExpenseBusiness();
        
        
        public ActionResult AllNotifications(int? page)
        {
            
            const int pageSize = 5;
            int pageNo = (page ?? 1);
            var list = nb.GetAllNotifications().OrderByDescending(x=>x.NotID);
            return View(list.ToPagedList(pageNo,pageSize));
        }
        public ActionResult _NewNotifications(int? page)
        {
            const int pageSize = 3;
            int pageNo = (page ?? 1);
            var list = nb.GetAllNotifications();
            return View(nb.GetAllNotifications().ToPagedList(pageNo,pageSize));
        }

        public ActionResult RemoveNot(int? id)
        {
            //string fullpath = Request.Url.PathAndQuery;
            //string firststring = fullpath.Substring(fullpath.IndexOf("/") + 1);
            //string controller = firststring.Substring(0, firststring.IndexOf("/")).Trim();
            //string ActionMethod = firststring.Substring(firststring.IndexOf("/") + 1).Trim();

            var not = nb.GetbyID(Convert.ToInt16(id));
            not.Red = true;
            nb.UpdateNotification(not);
            
            return RedirectToAction("FirstView");

        }

        [Authorize(Roles = "Admin")]
        public ActionResult FirstView()
        {
            int ExpList = 0, exppaid = 0, partner = 0, spon = 0, emp = 0, member = 0, pastor = 0, b = 0;

            int x = db.Users.ToList().Count;
            object[] o = GetCount();
            List<BranchViewModel> allb = new List<BranchViewModel>();
            List<Income> incomelist = db.Incomes.ToList();
            ExpList= esb.GetAllExpenses().Count;
            exppaid= (from ex in exb.GetAllExpenses_Paid() where ex.date_paid.Month == GetNistTime().Month select ex).ToList().Count;
            partner= (from s in Db.GetAllDonor() where s.Description == "Partner" select s).ToList().Count;
            spon= (from s in Db.GetAllDonor() where s.Description == "Sponsor" select s).ToList().Count;
            emp= eb.GetAllEmployee().Count;
            member= mb.GetAllMembers().Count;
            pastor= pb.GetAllPastors().Count;
            b = bb.GetAllBranches().Count;

            decimal Tincome= incomelist[0].TotIncome;

            ViewBag.income = Tincome;
            ViewBag.Allexp = ExpList;
            ViewBag.exp = exppaid;
            ViewBag.emp = emp;
            ViewBag.partner = partner;
            ViewBag.sponsor = spon;
            ViewBag.member = member;
            ViewBag.pastor = pastor;
            ViewBag.branches = b;
            ViewBag.all = o[0];
            ViewBag.today = o[1];
            return View();
        }
        public void Addvisitor(string ip, DateTime date)
        {
            
            VisitorCountsViewModel model = new VisitorCountsViewModel();
            model.Date =date;
            model.IpAddress = ip;

            var v = _vb.GetAllVisitors().Where(a => a.IpAddress.Equals(model.IpAddress)).FirstOrDefault();

            if (v == null)
            {
                _vb.AddVisitor(model);
            }
            else
            {
                _vb.UpdateVisitor(model);
            }
        }

        public object[] GetCount()
        {
            object[] o = new object[2];
            o[0] = _vb.GetAllVisitors().Count;
            o[1] = (from v in _vb.GetAllVisitors() where v.Date.Day==GetNistTime().Day select  v).Count();
            return o;
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