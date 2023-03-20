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
    public class AdminPaidExpenseController : Controller
    {
        Expense_PaidBusiness eb = new Expense_PaidBusiness();
        ExpenseBusiness exb = new ExpenseBusiness();
        PaidExpenseReceiptBusiness _perb = new PaidExpenseReceiptBusiness();
        ApplicationDbContext db = new ApplicationDbContext();

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

        public JsonResult GetPaidExpense(string term)
        {
            var employee = eb.GetAllExpenses_Paid().Where(x => x.ExpenseDesc.ToUpper().StartsWith(term.ToUpper())).Select(y => y.ExpenseDesc).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult GetAllPaidExpense(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = eb.GetAllExpenses_Paid().OrderByDescending(x => x.paid_exp_id);

            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult GetAllPaidExpense(string searchTerm, int? page)
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
        [Authorize(Roles = "Admin")]
        public ActionResult AddPaidExpense()
        {
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");

            return View();
        }
        [HttpPost]
        public ActionResult AddPaidExpense(Expense_PaidViewModel model, HttpPostedFileBase file)
        {

            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            try
            {
                ExpenseViewModel exp = exb.GetAllExpenses().Find(x => x.exp_id == model.exp_id);
                    if (model.amount_paid <= exp.amount)
                    {
                        model.Receipt = readFileContents(file);
                        eb.AddExpense_Paid(model);
                        SubtractIncome(model.amount_paid);

                        return RedirectToAction("GetAllPaidExpense");
                    }
                    ViewBag.feed = "Amount paid cannot exceeds the expense amount...!!";
            }
            catch (Exception d)
            {
                ViewBag.feed = d.Message;
            }
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            Expense_PaidViewModel evm = eb.GetbyID(Convert.ToInt32(id));

            return View(evm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UpdatePaidExpense(int? id)
        {
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            Expense_PaidViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePaidExpense(Expense_PaidViewModel model, HttpPostedFileBase File)
        {
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            try
            {
                Expense_PaidViewModel e = eb.GetAllExpenses_Paid().Find(x => x.paid_exp_id == model.paid_exp_id);

                ExpenseViewModel exp = exb.GetAllExpenses().Find(x => x.exp_id == e.exp_id);

                decimal totpayed = model.amount_paid + e.amount_paid;
                var fileName = Path.GetFileName(File.FileName + ".pdf");
                var fileName2 = Path.GetFileName(File.FileName);

                    if (totpayed <= exp.amount)
                    {
                        model.Receipt = readFileContents(File);

                        model.amount_paid = model.amount_paid + e.amount_paid;

                        eb.UpdateExpense_Paid(model);
                        SubtractIncome(model.amount_paid);

                        return RedirectToAction("GetAllPaidExpense");
                    }
                    ViewBag.feed = "Amount paid cannot exceeds the expense amount...!! ";

            }
            catch (Exception f)
            {

            }
            ViewBag.m = new SelectList(exb.GetAllExpenses(), "exp_id", "desc");
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeletePaidExpense(int? id)
        {
            Expense_PaidViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        public ActionResult DeletePaidExpense(int id)
        {
            try
            {
                Expense_PaidViewModel ep = eb.GetbyID(id);
                eb.DeleteExpense_Paid(id);
                return RedirectToAction("GetAllPaidExpense");
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

            PaidExpenseReceiptViewModel peb = _perb.GetbyID(Convert.ToInt32(id));
            Expense_PaidViewModel ep = eb.GetAllExpenses_Paid().Find(x => x.paid_exp_id == peb.paid_exp_id);
            ExpenseViewModel ee = exb.GetAllExpenses().Find(x => x.exp_id == ep.exp_id);
            try
            {
                _perb.DeleteExpense_PaidReceiptOnly(Convert.ToInt32(id));

                return RedirectToAction("ExpenseReceipt", new { id = ep.paid_exp_id });
            }
            catch (Exception c)
            {

            }
            return View("GetAllPaidExpense");
        }
    }
}