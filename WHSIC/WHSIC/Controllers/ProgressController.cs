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
    public class ProgressController : Controller
    {
        ProjectProgressBusiness ppb = new ProjectProgressBusiness();
        ProjectBusiness pb = new ProjectBusiness();
        InternetTime it = new InternetTime();
        [Authorize(Roles = "Secretary")]
        public ActionResult GetAllProgress(int projectID)
        {
            List<ProjectProgressViewModel> list = (from l in ppb.GetAllProjectProgresses()
                                                   where l.ProjectID == projectID
                                                   select l).ToList();
            var proj = pb.GetbyID(projectID);
            ViewBag.projName = proj.ProjectName;
            ViewBag.projeID = projectID;

            return View(list);
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult WeeklyProgress(int projectID)
        {
            ViewBag.feed = null;
            ViewBag.projIDW = projectID;
            return View();
        }
        [HttpPost]
        public ActionResult WeeklyProgress(ProjectProgressViewModel model, FormCollection form)
        {
            int projectID = Convert.ToInt32(form["passIDW"]);
            try
            {
                ViewBag.projIDW = projectID;
                List<ProjectProgressViewModel> progress = ppb.GetAllProjectProgresses().FindAll(x => x.ProjectID == projectID).ToList();

                ProjectProgressViewModel search = progress.Find(x => x.Duration == model.Duration);
                ProjectViewModel proj = pb.GetbyID(projectID);

                if (progress.Count == proj.Duration)
                {
                    ViewBag.feed = "This project has reached its maximum progress duration";
                }
                else
                {
                    if (search == null)
                    {
                        if (progress.Count == 0)
                        {
                            model.ProjectID = Convert.ToInt32(form["passIDW"]);
                            model.Date = it.GetNistTime();
                            ppb.AddProjectProgresses(model);
                            return RedirectToAction("GetAllProgress", new { projectID });
                        }
                        else
                        {
                            string currentvalue = null;
                            ProjectProgressViewModel last = progress.Last();
                            if (last.Duration == "Week 1")
                            {
                                currentvalue = "Week 2";
                            }
                            else if (last.Duration == "Week 2")
                            {
                                currentvalue = "Week 3";
                            }
                            else if (last.Duration == "Week 3")
                            {
                                currentvalue = "Week 4";
                            }
                            else if (last.Duration == "Week 4")
                            {
                                currentvalue = "Week 1";
                            }

                            if (currentvalue == model.Duration)
                            {
                                model.ProjectID = Convert.ToInt32(form["passIDW"]);
                                model.Date = it.GetNistTime();
                                ppb.AddProjectProgresses(model);
                                return RedirectToAction("GetAllProgress", new { projectID });
                            }
                            else
                            {
                                ViewBag.feed = "You must enter the progress for " + currentvalue + " before entering the progress for " + model.Duration + "..!";
                            }
                        }

                    }
                    else
                    {
                        ViewBag.feed = "Progress for " + model.Duration + " already exist, choose another week";
                        return View(model);
                    }
                }

            }
            catch (Exception v)
            {
                ViewBag.feed = v.Message;
            }

            ViewBag.projIDW = projectID;
            return View(model);
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult MonthlyProgress(int projectID)
        {
            ViewBag.feed = null;
            ViewBag.projIDM = projectID;
            return View();
        }
        [HttpPost]
        public ActionResult MonthlyProgress(ProjectProgressViewModel model, FormCollection form)
        {
            string feed = null;
            ViewBag.projIDM = form["passIDM"];
            int projectID = Convert.ToInt32(form["passIDM"]);

            ProjectViewModel project = pb.GetbyID(projectID);

            try
            {
                if (model.Duration == "April" && Convert.ToInt32(model.PeroidWorked) > 30)
                {
                    feed = "invalid Number of days... April ends on 30th";

                }
                else if (model.Duration == "June" && Convert.ToInt32(model.PeroidWorked) > 30)
                {
                    feed = "invalid Number of days... June ends on 30th";

                }
                else if (model.Duration == "September" && Convert.ToInt32(model.PeroidWorked) > 30)
                {
                    feed = "invalid Number of days... September ends on 30th";
                }
                else if (model.Duration == "November" && Convert.ToInt32(model.PeroidWorked) > 30)
                {
                    feed = "invalid Number of days... November ends on 30th";

                }
                else if (model.Duration == "February" && Convert.ToInt32(model.PeroidWorked) > FebDays())
                {
                    feed = "invalid Number of days... this Year February ends on " + FebDays() + "th";
                }
                else
                {
                    List<ProjectProgressViewModel> progress = ppb.GetAllProjectProgresses().FindAll(x => x.ProjectID == projectID).ToList();
                    ProjectProgressViewModel search = progress.Find(x => x.Duration == model.Duration);

                    ProjectViewModel proj = pb.GetbyID(projectID);

                    if (progress.Count == proj.Duration)
                    {
                        feed = "This project has reached its maximum progress duration";
                    }
                    else
                    {
                        if (search == null)
                        {
                            if (progress.Count == 0)
                            {


                                int smonth = project.StartDate.Month;
                                if (model.Duration == "January" && smonth != 1)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "February" && smonth != 2)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "March" && smonth != 3)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "April" && smonth != 4)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "May" && smonth != 5)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "June" && smonth != 6)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "July" && smonth != 7)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "August" && smonth != 8)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "September" && smonth != 9)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "October" && smonth != 10)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "November" && smonth != 11)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else if (model.Duration == "December" && smonth != 12)
                                {
                                    feed = "You selected invalid Month, this project starts on " + project.StartDate.ToShortDateString() + ", enter a month that matches with project start month";
                                    ViewBag.feed = feed;
                                    return View();
                                }
                                else
                                {
                                    model.ProjectID = projectID;
                                    model.Date = it.GetNistTime();
                                    ppb.AddProjectProgresses(model);
                                    return RedirectToAction("GetAllProgress", new { projectID });
                                }

                                

                            }
                            else
                            {
                                string currentvalue = null;
                                ProjectProgressViewModel last = progress.Last();

                                if (last.Duration == "January")
                                {
                                    currentvalue = "February";
                                }
                                else if (last.Duration == "February")
                                {
                                    currentvalue = "March";
                                }
                                else if (last.Duration == "March")
                                {
                                    currentvalue = "April";
                                }
                                else if (last.Duration == "April")
                                {
                                    currentvalue = "May";
                                }
                                else if (last.Duration == "May")
                                {
                                    currentvalue = "June";
                                }
                                else if (last.Duration == "June")
                                {
                                    currentvalue = "July";
                                }
                                else if (last.Duration == "July")
                                {
                                    currentvalue = "August";
                                }
                                else if (last.Duration == "August")
                                {
                                    currentvalue = "September";
                                }
                                else if (last.Duration == "September")
                                {
                                    currentvalue = "October";
                                }
                                else if (last.Duration == "October")
                                {
                                    currentvalue = "November";
                                }
                                else if (last.Duration == "November")
                                {
                                    currentvalue = "December";
                                }
                                else if (last.Duration == "December")
                                {
                                    currentvalue = "January";
                                }



                                if (model.Duration == currentvalue)
                                {
                                    model.ProjectID = projectID;
                                    model.Date = it.GetNistTime();
                                    ppb.AddProjectProgresses(model);
                                    ViewBag.projIDM = projectID;
                                    return RedirectToAction("GetAllProgress", new { projectID });
                                }
                                else
                                {
                                    feed = "You must enter the progress for " + currentvalue + " before entering the progress for " + model.Duration + "..!";
                                }
                            }

                        }
                        else
                        {
                            feed = "Progress for " + model.Duration + " already exist, choose another Month..!";
                        }
                    }
                }
            }
            catch (Exception v)
            {
                feed = v.Message;
            }
            ViewBag.projIDM = projectID;
            ViewBag.feed = feed;
            return View(model);
        }

        public int FebDays()
        {
            return DateTime.DaysInMonth(it.GetNistTime().Year, 2);
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult YearlyProgress(int projectID)
        {
            ViewBag.feed = null;
            ViewBag.projIDY = projectID;
            ViewBag.years = new SelectList(GenerateYears(), "year", "year");
            return View();
        }
        [HttpPost]
        public ActionResult YearlyProgress(ProjectProgressViewModel model, FormCollection form)
        {
            string feed = null;
            int projectID = Convert.ToInt32(form["passIDY"]);
            try
            {
                List<ProjectProgressViewModel> progress = ppb.GetAllProjectProgresses().FindAll(x => x.ProjectID == projectID).ToList();
                ProjectProgressViewModel search = progress.Find(x => x.Duration == model.Duration);
                ProjectViewModel proj = pb.GetbyID(projectID);

                if (progress.Count == proj.Duration)
                {
                    feed = "This project has reached its maximum progress duration";
                }
                else
                {
                    if (search == null)
                    {
                        if (progress.Count == 0)
                        {
                            model.ProjectID = Convert.ToInt32(form["passIDW"]);
                            model.Date = it.GetNistTime();
                            ppb.AddProjectProgresses(model);
                            return RedirectToAction("GetAllProgress", new { projectID });
                        }
                        else
                        {
                            int currentvalue = Convert.ToInt32(progress.Last().Duration);

                            if (Convert.ToInt32(model.Duration) == currentvalue + 1)
                            {
                                model.ProjectID = projectID;
                                model.Date = it.GetNistTime();
                                ppb.AddProjectProgresses(model);
                                return RedirectToAction("GetAllProgress", new { projectID });
                            }
                            else
                            {
                                currentvalue = currentvalue + 1;
                                feed = "You must enter the progress for " + currentvalue + " before entering the progress for " + model.Duration + "..!";
                            }
                        }
                    }
                    else
                    {
                        feed = "Progress for " + model.Duration + " already exist, choose another year..!";
                    }
                }
                ViewBag.years = new SelectList(GenerateYears(), "year", "year");
            }
            catch (Exception v)
            {
                feed = v.Message;
            }
            ViewBag.projIDY = projectID;
            ViewBag.feed = feed;
            return View(model);
        }
        public List<TempYears> GenerateYears()
        {
            int y = it.GetNistTime().Year;
            y = y - 1;
            List<TempYears> hold_years = new List<TempYears>();
            for (int c = 0; c < 10; c++)
            {

                TempYears year = new TempYears
                {
                    year = (y + c).ToString()
                };

                hold_years.Add(year);
            }

            return hold_years;
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult AddProgress(int projectID)
        {
            ProjectViewModel p = pb.GetbyID(projectID);
            if (p.DurationFormat == "Month(s)")
            {
                return RedirectToAction("MonthlyProgress", new { projectID = p.ProjectID });
            }
            else if (p.DurationFormat == "Year(s)" || p.Duration > 12)
            {
                return RedirectToAction("YearlyProgress", new { projectID = p.ProjectID });
            }
            else if (p.DurationFormat == "Week(s)")
            {
                return RedirectToAction("WeeklyProgress", new { projectID = p.ProjectID });
            }
            return View("GetAllProject", "Project");
        }

        public ActionResult ChooseGraph(int projectID)
        {
            ProjectViewModel p = pb.GetbyID(projectID);
            if (p.DurationFormat == "Month(s)")
            {
                return RedirectToAction("MonthlyGraph", new { projectID = p.ProjectID });
            }
            else if (p.DurationFormat == "Year(s)" || p.Duration > 12)
            {
                return RedirectToAction("YearlyGraph", new { projectID = p.ProjectID });
            }
            else if (p.DurationFormat == "Week(s)")
            {
                return RedirectToAction("WeeklyGraph", new { projectID = p.ProjectID });
            }
            return View();
        }

        //Graphs
        public ActionResult YearlyGraph(int projectID)
        {
            ViewBag.proj = projectID;
            return View();
        }
        public ActionResult MonthlyGraph(int projectID)
        {
            ViewBag.proj = projectID;
            return View();
        }
        public ActionResult WeeklyGraph(int projectID)
        {
            ViewBag.proj = projectID;
            return View();
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult EditProgress(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectProgressViewModel ppv = ppb.GetbyID(Convert.ToInt32(id));
            if (ppv == null)
            {
                return HttpNotFound();
            }
            ViewBag.projID = ppv.ProjectID;
            return View(ppv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProgress(ProjectProgressViewModel model)
        {
            ViewBag.projID = model.ProjectID;
            if (ModelState.IsValid)
            {
                model.Date = it.GetNistTime();
                ppb.UpdateProjectProgresses(model);
                return RedirectToAction("GetAllProgress", new { projectID = model.ProjectID });
            }
            ViewBag.projID = model.ProjectID;
            return View(model);
        }
        public ActionResult DeleteProgress(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectProgressViewModel ppv = ppb.GetbyID(Convert.ToInt32(id));
            if (ppv == null)
            {
                return HttpNotFound();
            }
            return View(ppv);
        }
        [HttpPost, ActionName("DeleteProgress")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectProgressViewModel ppv = ppb.GetbyID(id);
            ppb.DeleteProjectProgresses(ppv.ProgressID);
            return RedirectToAction("GetAllProgress", new { projectID = ppv.ProjectID });
        }

    }
}