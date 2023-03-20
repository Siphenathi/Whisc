using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class Expense_PaidController : Controller
    {
        Expense_PaidBusiness eb = new Expense_PaidBusiness();
        ExpenseBusiness exb = new ExpenseBusiness();
        PaidExpenseReceiptBusiness _perb = new PaidExpenseReceiptBusiness();
        ApplicationDbContext db = new ApplicationDbContext();
        private NotificationBusiness nb = new NotificationBusiness();
        private EmployeeBusiness _eb = new EmployeeBusiness();
        private string Report = "";

        public FileStreamResult Showfile(int? id)
        {
            Stream output = new MemoryStream(getContract((int)id).Receipt);
            return new FileStreamResult(output, "application/pdf");
        }
        public PaidExpenseReceiptViewModel getContract(int id)
        {
            return _perb.GetbyID(id);
        }
        private byte[] readFileContents(HttpPostedFileBase file)
        {
            Stream fileStream = file.InputStream;
            var mStreamer = new MemoryStream();
            mStreamer.SetLength(fileStream.Length);
            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
            mStreamer.Seek(0, SeekOrigin.Begin);
            byte[] fileBytes = mStreamer.GetBuffer();
            return fileBytes;
        }

        public JsonResult GetEmployees(string term)
        {
            var employee = eb.GetAllExpenses_Paid().Where(x => x.ExpenseDesc.ToUpper().StartsWith(term.ToUpper())).Select(y => y.ExpenseDesc).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Treasure")]
        public ActionResult Index(int?page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = eb.GetAllExpenses_Paid().OrderByDescending(x=>x.paid_exp_id);

            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchTerm, int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            List<Expense_PaidViewModel> Employee;

            if (string.IsNullOrEmpty(searchTerm))
            {
                Employee = eb.GetAllExpenses_Paid();
            }
            else
            {
                Employee = eb.GetAllExpenses_Paid().Where(x => x.ExpenseDesc.StartsWith(searchTerm)).ToList();
            }
            var list = Employee;
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public List<ExpenseViewModel>ExpenseName()
        {
            List<ExpenseViewModel> myList = (from e in exb.GetAllExpenses() where e.recurring == false select e).ToList();
            return myList;
        }
        public void SubtractIncome(decimal amt)
        {
            decimal val = 0;

            foreach (var CO in db.Incomes.ToList())
            {
                val = CO.TotIncome;
                val = val - amt;
                CO.TotIncome = val;
                db.SaveChanges();
                break;
            }
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult Create()
        {
            ViewBag.m = new SelectList(ExpenseName(), "exp_id", "desc");

            return View();
        }
        [HttpPost]
        public ActionResult Create(Expense_PaidViewModel model, HttpPostedFileBase file)
        {
            EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);

            ViewBag.m = new SelectList(ExpenseName(), "exp_id", "desc");
            try
            {
                ExpenseViewModel exp = exb.GetAllExpenses().Find(x => x.exp_id == model.exp_id);
                    if (model.amount_paid <= exp.amount)
                    {
                        model.Receipt = readFileContents(file);
                        eb.AddExpense_Paid(model);
                        SubtractIncome(model.amount_paid);
                        Report = emp.FirstName + " " + emp.LastName + " has added the payment record for " + exp.desc + " expense";

                        AddNotification(Report);
                        return RedirectToAction("Index");
                    }
                    ViewBag.feed = "Amount paid cannot exceeds the expense amount...!!";
            }
            catch (Exception d)
            {
                ViewBag.feed = d.Message;
            }
            ViewBag.m = new SelectList(ExpenseName(), "exp_id", "desc");
            return View();
        }
        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Details(int id)
        {
            Expense_PaidViewModel evm = eb.GetbyID(Convert.ToInt32(id));

            return View(evm);
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult Edit(int? id)
        {
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            Expense_PaidViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Expense_PaidViewModel model, HttpPostedFileBase File)
        {
            EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            try
            {
                Expense_PaidViewModel e = eb.GetAllExpenses_Paid().Find(x => x.paid_exp_id == model.paid_exp_id);

                ExpenseViewModel exp = exb.GetAllExpenses().Find(x => x.exp_id == e.exp_id);

                decimal totpayed = model.amount_paid + e.amount_paid;

                    if (totpayed <= exp.amount)
                    {
                        model.Receipt = readFileContents(File); 

                        model.amount_paid = model.amount_paid + e.amount_paid;

                        eb.UpdateExpense_Paid(model);
                        SubtractIncome(model.amount_paid);

                        Report = emp.FirstName + " " + emp.LastName + " has updated the payment record for " + model.ExpenseDesc + " expense";

                        AddNotification(Report);

                        return RedirectToAction("Index");
                    }
                    ViewBag.feed = "Amount paid cannot exceeds the expense amount...!! ";
               
            }
            catch (Exception f)
            {

            }
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            return View(model);
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
        public ActionResult Delete(int? id)
        {
            Expense_PaidViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);
            try
            {
                Expense_PaidViewModel ep = eb.GetbyID(id);
                eb.DeleteExpense_Paid(id);

                Report = emp.FirstName + " " + emp.LastName + " has deleted the payment record for "+ ep.ExpenseDesc+ " expense";

                AddNotification(Report);
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {

            }
            return View();
        }

        public ActionResult ExpenseReceipt(int id)
        {
            var Reciepts = (from r in _perb.GetAllExpenses_PaidReceipt() where r.paid_exp_id == id select r).ToList();
            ViewBag.count = Reciepts.Count;
            return View(Reciepts);
        }

        public ActionResult DeleteReceipt(int? id)
        {
            EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);
           
            PaidExpenseReceiptViewModel peb = _perb.GetbyID(Convert.ToInt32(id));
            Expense_PaidViewModel ep = eb.GetAllExpenses_Paid().Find(x=>x.paid_exp_id==peb.paid_exp_id);
            ExpenseViewModel ee = exb.GetAllExpenses().Find(x => x.exp_id == ep.exp_id);
            try
            {
                _perb.DeleteExpense_PaidReceiptOnly(Convert.ToInt32(id));

                Report = emp.FirstName + " " + emp.LastName + " has deleted the receipt of the payment record for " + ee.desc + " expense";
                AddNotification(Report);
                return RedirectToAction("ExpenseReceipt",new {id=ep.paid_exp_id});
            }
            catch(Exception c)
            { 
                   
            }
            return View("Index");
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