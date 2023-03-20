using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class SalaryController : Controller
    {
        SalaryBusiness sb = new SalaryBusiness();
        EmployeeBusiness eb = new EmployeeBusiness();

        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Index(int ?page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = sb.GetAllSalary();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public List<EmployeeViewModel> namelist(List<EmployeeViewModel> list)
        {
            List<EmployeeViewModel> mylist = new List<EmployeeViewModel>();

            foreach (EmployeeViewModel p in list)
            {
                EmployeeViewModel pa = new EmployeeViewModel
                {
                    FirstName = p.FirstName + " " + p.LastName,
                    EmployeeNo=p.EmployeeNo
                    
                };

                mylist.Add(pa);
            }
            return mylist;
        }
        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Create()
        {
            ViewBag.empNo = new SelectList(namelist(eb.GetAllEmployee()), "EmployeeNo", "FirstName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(SalaryViewModel model)
        {
            ViewBag.empNo = new SelectList(namelist(eb.GetAllEmployee()), "EmployeeNo", "FirstName");
            try
            {
                sb.AddSalary(model);
               return  RedirectToAction("Index");
            }
            catch(Exception d)
            {
                Response.Write(d.Message);
            }
            ViewBag.empNo = new SelectList(namelist(eb.GetAllEmployee()), "EmployeeNo", "FirstName");
            return View(model);
        }
        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Delete(int?id)
        {
            SalaryViewModel sal = sb.GetbyID(Convert.ToInt32(id));
            return View(sal);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                sb.DeleteSalary(id);
                return RedirectToAction("Index");

            }
            catch(Exception c)
            {
                Response.Write(c.Message);
            }
            return View();
        }
        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Edit(int? id)
        {
            ViewBag.empNo = new SelectList(namelist(eb.GetAllEmployee()), "EmployeeNo", "FirstName");
            SalaryViewModel sal = sb.GetbyID(Convert.ToInt32(id));
            return View(sal);
        }
        [HttpPost]
        public ActionResult Edit(SalaryViewModel model)
        {
            ViewBag.empNo = new SelectList(namelist(eb.GetAllEmployee()), "EmployeeNo", "FirstName");
            try
            {
                sb.UpdateSalary(model);
                return RedirectToAction("Index");
            }
            catch(Exception c)
            {
                Response.Write(c.Message);
            }
            return View(model);
        }
        public ActionResult Details(int?id)
        {
            SalaryViewModel sal = sb.GetbyID(Convert.ToInt32(id));
            return View(sal);
        }
    }
}