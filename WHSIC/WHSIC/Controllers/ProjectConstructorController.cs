using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class ProjectConstructorController : Controller
    {
        ProjectConstructorsBusiness pcb = new ProjectConstructorsBusiness();

        public ActionResult GetAllProjectConstructors()
        {
            return View();
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult CreateProjectConstructors(int projectID)
        {
            ProjectConstructorViewModel pcv = new ProjectConstructorViewModel
            {
                ProjectID = projectID
            };
            return View(pcv);
        }
        [HttpPost]
        public ActionResult CreateProjectConstructors(ProjectConstructorViewModel model)
        {
            try
            {
                pcb.AddProjectConstructor(model);

                ProjectConstructorViewModel last = pcb.GetAllProjectonstructors().Last();

                return RedirectToAction("AddProjectInvoice", "ProjectInvoice", new { projectID =model.ProjectID, ConstructorID=last.ContructorID });
            }
            catch(Exception v)
            {
                
            }
            return View(model);
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult EditConstructors(int? id)
        {
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectConstructorViewModel ppv = pcb.GetAllProjectonstructors().Find(x=>x.ContructorID==Convert.ToInt32(id));
            if (ppv == null)
            {
                return HttpNotFound();
            }
            return View(ppv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConstructors(ProjectConstructorViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    pcb.UpdateProjectonstructor(model);
                    return RedirectToAction("ProjectDetails", "Project", new { id=model.ProjectID});
                }
              
            }
            catch(Exception v)
            {
                ViewBag.feed = v.Message;
            }
            return View(model);
        }
    }
}