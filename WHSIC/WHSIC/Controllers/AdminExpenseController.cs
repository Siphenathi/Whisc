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
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class AdminExpenseController : Controller
    {
        private ExpenseBusiness eb = new ExpenseBusiness();
        private EmployeeBusiness _eb = new EmployeeBusiness();
        private Expense_PaidBusiness paidexp = new Expense_PaidBusiness();

        [Authorize(Roles = "Admin")]

        public JsonResult GetExpense(string term)
        {
            List<string> employee = eb.GetAllExpenses().Where(x => x.desc.ToUpper().StartsWith(term.ToUpper()))
                .Select(y => y.desc).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult GetAllExpense(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = eb.GetAllExpenses().OrderByDescending(x => x.exp_id);
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult GetAllExpense(string searchTerm, int? page)
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


        [Authorize(Roles = "Admin")]
        public ActionResult AddExpense()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        public ActionResult AddExpense(ExpenseViewModel model)
        {
            EmployeeViewModel emp = _eb.GetAllEmployee().Find(x => x.Email == User.Identity.Name);
            ExpenseViewModel exp = eb.GetAllExpenses().Find(x => x.desc.ToUpper() == model.desc.ToUpper());
            try
            {
                if (exp == null)
                {
                    eb.AddExpense(model);
                    return RedirectToAction("GetAllExpense");
                }
                ViewBag.feed = model.desc + " expense already exist, use another name";
            }
            catch (Exception d)
            {

            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            ExpenseViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateExpense(int? id)
        {
            ViewBag.feed = null;
            ExpenseViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        public ActionResult UpdateExpense(ExpenseViewModel model)
        {
        
            try
            {
                    eb.UpdateExpense(model);
                    return RedirectToAction("GetAllExpense");
            }
            catch (Exception f)
            {
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteExpense(int? id)
        {
            ViewBag.feed = null;
            ExpenseViewModel evm = eb.GetbyID(Convert.ToInt32(id));
            return View(evm);
        }
        [HttpPost]
        public ActionResult DeleteExpense(int id)
        {
            try
            {
                ExpenseViewModel exp = eb.GetbyID(id);

                List<Expense_PaidViewModel> paidexpList = paidexp.GetAllExpenses_Paid().Where(x => x.exp_id == exp.exp_id).ToList();

                if (paidexpList.Count == 0)
                {
                    eb.DeleteExpense(id);
                    return RedirectToAction("GetAllExpense");
                }
                ViewBag.feed = "The are paid expense records that reference this expense, firstly delete all records that reference this expense";
            }
            catch (Exception b)
            {

            }
            return View();
        }

    }
}