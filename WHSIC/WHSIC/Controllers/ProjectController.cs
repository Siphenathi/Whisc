using PagedList;
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
    public class ProjectController : Controller
    {
        ProjectBusiness pb = new ProjectBusiness();
        ProjectProgressBusiness ppb = new ProjectProgressBusiness();
        ProjectInvoicesBusiness pib = new ProjectInvoicesBusiness();
        [Authorize(Roles = "Secretary")]
        public ActionResult GetAllProject(int ?page)
        {
            List<ProjectViewModel> list = (from p in pb.GetAllProjects() orderby p.ProjectID descending select p).ToList();
            const int pageSize = 6;
            int pageNo = (page ?? 1);
            return View(list.ToPagedList(pageNo, pageSize));
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult AddProject()
        {
            ViewBag.feed = null;
            return View();
        }

        [HttpPost]
        public ActionResult AddProject(ProjectViewModel model)
        {
            try
            {
                ProjectViewModel proj = pb.GetAllProjects().Find(x => x.ProjectName.ToLower().Equals(model.ProjectName.ToLower()));
                if (proj != null)
                {
                    ViewBag.feed = "There is already a project with the same name as " + model.ProjectName + " ,choose another name";
                }
                else
                {
                    if (model.Duration > 0)
                    {
                        int duWeeks = (Convert.ToInt16(model.Duration)) * 7;

                        if (model.DurationFormat == "Month(s)")
                        {
                            model.EndDate = model.StartDate.AddMonths(model.Duration);
                        }
                        else if (model.DurationFormat == "Week(s)")
                        {
                            model.EndDate = model.StartDate.AddDays(duWeeks);

                        }
                        else if (model.DurationFormat == "Days")
                        {
                            model.EndDate = model.StartDate.AddDays(model.Duration);
                        }
                        else if (model.DurationFormat == "Year(s)")
                        {
                            model.EndDate = model.StartDate.AddYears(model.Duration);
                        }
   
                            if (model.DurationFormat == "Month(s)" && Convert.ToInt32(model.Duration) > 12)
                            {
                                ViewBag.feed = "Invalid Duration, duration for months cannot be greater than 12";
                                
                            }
                            else if (model.DurationFormat == "Week(s)" && Convert.ToInt32(model.Duration) > 4)
                            {
                                ViewBag.feed = "Invalid Duration, duration for weeks cannot be greater than 4";
                                
                            }
                            else if (model.DurationFormat == "Year(s)" && Convert.ToInt32(model.Duration) > 10)
                            {
                                ViewBag.feed = "Invalid Duration, duration for years cannot be greater than 10";
                              
                            }
                            else
                            {
                                pb.AddProject(model);

                                ProjectViewModel pro = pb.GetAllProjects().Last();

                                return RedirectToAction("CreateProjectConstructors", "ProjectConstructor", new { projectID = pro.ProjectID });
                            }
                    }
                    else
                    {
                        ViewBag.feed = "Invalid Duration, duration cannot be below 1";
                    }
                }
            }
            catch (Exception c)
            {
                ViewBag.feed = c.Message;
            }
            return View(model);
        }
        public ActionResult Finish(int projectID)
        {
            ViewBag.proj = projectID;

            return View();
        }
        public ActionResult Progress(int projectID)
        {
            return RedirectToAction("AddProgress", "Progress", new { projectID });
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult DeleteProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectViewModel proj = pb.GetbyID(Convert.ToInt32(id));
            if (proj == null)
            {
                return HttpNotFound();
            }
            return View(proj);
        }
        [HttpPost]
        public ActionResult DeleteProject(int id)
        {
            try
            {
                pb.DeleteProject(id);
                return RedirectToAction("GetAllProject");
            }
            catch (Exception b)
            {

            }
            return View();
        }

        public ActionResult ProjectDetails(int id)
        {
            ProjectViewModel proj = pb.GetbyID(id);
            ViewBag.projID = proj.ProjectID;
            return View();
        }
        public ActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectViewModel proj = pb.GetbyID(Convert.ToInt32(id));
            if (proj == null)
            {
                return HttpNotFound();
            }
            return View(proj);
        }
        [HttpPost]
        public ActionResult EditProject(ProjectViewModel model)
        {
            try
            {
                
                if (model.Duration > 0)
                {
                    int duWeeks = (Convert.ToInt16(model.Duration)) * 7;

                    if (model.DurationFormat == "Month(s)")
                    {
                        model.EndDate = model.StartDate.AddMonths(model.Duration);
                    }
                    else if (model.DurationFormat == "Week(s)")
                    {
                        model.EndDate = model.StartDate.AddDays(duWeeks);

                    }
                    else if (model.DurationFormat == "Days")
                    {
                        model.EndDate = model.StartDate.AddDays(model.Duration);
                    }
                    else if (model.DurationFormat == "Year(s)")
                    {
                        model.EndDate = model.StartDate.AddYears(model.Duration);
                    }
                   
                        if (model.DurationFormat == "Month(s)" && Convert.ToInt32(model.Duration) > 12)
                        {
                            ViewBag.feed = "Invalid Duration, duration for months cannot be greater than 12";
                            return View(model);
                        }
                        else if (model.DurationFormat == "Week(s)" && Convert.ToInt32(model.Duration) > 4)
                        {
                            ViewBag.feed = "Invalid Duration, duration for weeks cannot be greater than 4";
                            return View(model);
                        }
                        else if (model.DurationFormat == "Year(s)" && Convert.ToInt32(model.Duration) > 10)
                        {
                            ViewBag.feed = "Invalid Duration, duration for years cannot be greater than 10";
                            return View(model);
                        }
                        else
                        {
                            pb.UpdateProject(model);

                            ProjectViewModel pro = pb.GetAllProjects().Last();

                            return RedirectToAction("GetAllProject");
                        }
                }
                else
                {
                    ViewBag.feed = "Invalid Duration, duration cannot be below 1";
                }
                
            }
            catch (Exception v)
            {
                ViewBag.feed = v.Message;
            }
            return View(model);
        }
    }
}