using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;
namespace WHSIC.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeBusiness eb = new EmployeeBusiness();
        PositionBusiness pb = new PositionBusiness();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public EmployeeController()
        {

        }
        public EmployeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        //[Authorize(Roles = "Treasure, Admin")]
        public JsonResult GetEmployees(string term)
        {
            List<string> employee = eb.GetAllEmployee().Where(x => x.Email.ToUpper().StartsWith(term.ToUpper()))
                .Select(y => y.Email).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = eb.GetAllEmployee();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchTerm,int?page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            List<EmployeeViewModel> Employee;

            if (string.IsNullOrEmpty(searchTerm))
            {
                Employee = eb.GetAllEmployee();
            }
            else
            {
                Employee = eb.GetAllEmployee().Where(x => x.Email.StartsWith(searchTerm)).ToList();
            }
            var list = Employee;
            return View(list.ToPagedList(pageNo,pageSize));
        }

        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Create()
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            ViewBag.feed = null;
            return View();
        }

        [HttpPost]
        public  async Task<ActionResult> Create(EmployeeViewModel model)
        {
            string feed = null;
                ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
                try
                {
                    PositionViewModel p = pb.GetbyID(model.PositionID);
                    model.Description = p.Description;

                    if (model.Tax_No == null)
                    {
                        model.Tax_No = "Not Applicable";
                         EmployeeViewModel emp = eb.GetAllEmployee().Find(x => x.Email == model.Email);
                       if (emp == null)
                       {
                          eb.AddEmployee(model);
                           return RedirectToAction("Index");
                       }
                      else
                       {
                          feed = "Employee already exist ";

                        }
                     }
                    else
                    {
                        EmployeeViewModel emp = eb.GetAllEmployee().Find(x => x.Email == model.Email);
                        if(emp==null)
                        {
                            eb.AddEmployee(model);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            feed = "Employee already exist ";
                           
                        }
                        
                    }
                    
                }
                catch (Exception b)
                {
                    ViewBag.feed=b.Message;
                }
                ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
           
            ViewBag.feed = feed;
            return View();
        }
        //[Authorize(Roles = "Treasure, Admin")]
        public ActionResult Details(int id)
        {
            EmployeeViewModel ft = eb.GetbyID(id);
            return View(ft);
        }
        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Delete(int? id)
        {
            EmployeeViewModel ft = eb.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                eb.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }
        [Authorize(Roles = "Treasure, Admin")]
        public ActionResult Edit(int? id)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            EmployeeViewModel ft = eb.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            try
            {
                eb.UpdateEmployee(model);
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }
    }
}