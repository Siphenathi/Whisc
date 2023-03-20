using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class ExpenseController : Controller
    {
        private ExpenseBusiness eb = new ExpenseBusiness();
        private NotificationBusiness nb = new NotificationBusiness();
        private EmployeeBusiness _eb = new EmployeeBusiness();
        private Expense_PaidBusiness paidexp = new Expense_PaidBusiness();
        private string Report = "";

        [Authorize(Roles = "Treasure")]

        public JsonResult GetEmployees(string term)
        {
            List<string> employee = eb.GetAllExpenses().Where(x => x.desc.ToUpper().StartsWith(term.ToUpper()))
                .Select(y => y.desc).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = eb.GetAllExpenses().OrderByDescending(x=>x.exp_id);
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchTerm, int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            List<ExpenseViewModel> Program;

            if (string.IsNullOrEmpty(searchTerm))
            {
                Program = eb.GetAllExpenses();
            }
            else
            {
                Program = eb.GetAllExpenses().Where(x => x.desc.StartsWith(searchTerm)).ToList();
            }
            var list = Program;
            return View(list.ToPagedList(pageNo, pageSize));
        }


        [Authorize(Roles = "Treasure")]
        public ActionResult Create()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        public ActionResult Create(ExpenseViewModel model)
        {
            EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);
            ExpenseViewModel exp = eb.GetAllExpenses().Find(x => x.desc.ToUpper() == model.desc.ToUpper());
            try
            {
                if (exp == null)
                {
                    eb.AddExpense(model);
                    Report = emp.FirstName + " " + emp.LastName + " has added new expense, Name :" + model.desc;
                    AddNotification(Report);
                    return RedirectToAction("Index");
                }
                ViewBag.feed = model.desc + " expense already exist, use another name";
            }
            catch (Exception d)
            {

            }
            return View();
        }

        public void AddNotification(string reportp)
        {
            NotificationViewModel not = new NotificationViewModel
            {
                Text = reportp,
                DateTime = GetNistTime()
            };
            nb.AddNotification(not);
        }

        [Authorize(Roles = "Treasure")]
        public ActionResult Details(int id)
        {
            ExpenseViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult Edit(int ?id)
        {
            ViewBag.feed = null;
            ExpenseViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        public ActionResult Edit(ExpenseViewModel model)
        {
            EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);
            //ExpenseViewModel exp = eb.GetAllExpenses().Find(x => x.desc.ToUpper() == model.desc.ToUpper());
            try
            {
                //if (exp == null)
                //{
                    eb.UpdateExpense(model);
                    Report = emp.FirstName + " " + emp.LastName + " has updated " + model.desc + " expense";
                    return RedirectToAction("Index");
                //}
                //ViewBag.feed = model.desc + " expense already exist, use another name";


                
            }
            catch(Exception f)
            {

            }
            return View(model);
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult Delete(int? id)
        {
            ViewBag.feed = null;
            ExpenseViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);
                ExpenseViewModel exp = eb.GetbyID(id);

                List<Expense_PaidViewModel> paidexpList=paidexp.GetAllExpenses_Paid().Where(x => x.exp_id == exp.exp_id).ToList();

                if (paidexpList.Count==0)
                {
                    eb.DeleteExpense(id);

                    Report = emp.FirstName + " " + emp.LastName + " has deleted the " + exp.desc + " expense";
                    AddNotification(Report);
                    return RedirectToAction("Index");
                }
                ViewBag.feed = "The are paid expense records that reference this expense, firstly delete all records that reference this expense";
            }
            catch(Exception b)
            {

            }
            return View();
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
                    double milliseconds = Convert.ToInt64(time) / 1000.0;
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