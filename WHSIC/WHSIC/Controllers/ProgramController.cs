using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WHSIC.Data;
using WHSIC.Model;
using PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using WHSIC.BusinessLogic;
using WHSIC.Service;

namespace WHSIC.Controllers
{
    public class ProgramController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        ProgramBusiness pb = new ProgramBusiness();


        public JsonResult GetEmployees(string term)
        {
            List<string> employee = pb.GetAllProgramies().Where(x => x.PName.ToUpper().StartsWith(term.ToUpper()))
                .Select(y => y.PName).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = new ProgramListModel
            {
                ProgramList = pb.GetAllProgramies()
            };
            return View(list.ProgramList.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchTerm, int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            List<ProgramViewModel> Program;

            if (string.IsNullOrEmpty(searchTerm))
            {
                Program = pb.GetAllProgramies();
            }
            else
            {
                Program = pb.GetAllProgramies().Where(x => x.PName.StartsWith(searchTerm)).ToList();
            }
            var list = Program;
            return View(list.ToPagedList(pageNo, pageSize));
        }


        [Authorize(Roles = "Secretary")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProgramViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProgramViewModel f = pb.GetAllProgramies().Find(x => x.PName.ToLower().Equals(model.PName.ToLower()));
                    int ye = Convert.ToInt32(model.Date.Year);
                    int cy = Convert.ToInt32(DateTime.Now.Year);

                    if (ye <= cy)
                    {
                        if (f == null)
                        {
                            pb.AddProgram(model);
                            return RedirectToAction("Index");
                        }
                        else { ViewBag.m = "This Program already exist you can only edit it to make changes "; }
                    }
                    else { ViewBag.m = "Program cannot be launged in advance year, enter previous or current year"; }
                }
                catch (Exception b)
                {
                    Response.Write(b.Message);
                }

            }
            return View();

        }

        [Authorize(Roles = "Secretary")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramViewModel pc = pb.GetbyID(Convert.ToInt32(id));

            if (pc == null)
            {
                return HttpNotFound();
            }
            return View(pc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProgramViewModel model)
        {
            try
            {

                  Program p = db.Programs.ToList().Find(x=>x.PId==model.PId);

                    if (p != null)
                    {
                        p.PName = model.PName;
                        p.Date = model.Date.ToShortDateString();
                        p.Venue = model.Venue;
                        p.Frequence = model.Frequence;
                    }
                    db.SaveChanges();

                    return RedirectToAction("Index");
              
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

        [Authorize(Roles = "Secretary")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramViewModel pc = pb.GetbyID(Convert.ToInt32(id));

            if (pc == null)
            {
                return HttpNotFound();
            }
            return View(pc);

        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                pb.DeleteProgram(id);
                return RedirectToAction("Index");

            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramViewModel pc = pb.GetbyID(Convert.ToInt32(id));

            if (pc == null)
            {
                return HttpNotFound();
            }
            return View(pc);
        }

    }
}