using WHSIC.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using System.Threading.Tasks;

namespace WHSIC.Controllers
{
    public class SermonController : Controller
    {
        SermonBusiness sb = new SermonBusiness();
        PastorBusiness pb = new PastorBusiness();
        BranchBusiness bb = new BranchBusiness();
        FinalSermonBusiness fb = new FinalSermonBusiness();
        SMS sms = new SMS();

        public List<PastorViewModel> namelist(List<PastorViewModel> list)
        {
            List<PastorViewModel> mylist = new List<PastorViewModel>();

            foreach (PastorViewModel p in list)
            {
                PastorViewModel pa = new PastorViewModel
                {
                    FirstName = p.FirstName + " " + p.Surname
                };
                mylist.Add(pa);
            }
            return mylist;
        }

        public ActionResult Index(int? page, string feed)
        {
            const int pageSize = 8;
            int pageNo = (page ?? 1);

            var list = (from s in sb.GetAllSermons() select s).OrderByDescending(c => c.Date.Month);

            if (feed != null)
            {
                ViewBag.feed = feed;
            }

            return View(list.ToPagedList(pageNo, pageSize));
        }
        public string CurrentDate()
        {
            DateTime cm = DateTime.Now;


            string cmonth = cm.Month.ToString();

            if (cmonth.Length == 1)
            {
                cmonth = "0" + cmonth;
            }

            return DateTime.Now.Day.ToString() + cmonth + DateTime.Now.Year.ToString();
        }
        public string EnteredDate(DateTime edpar)
        {
            string m = " ";
            DateTime dt = new DateTime();
            dt = edpar;

            m = dt.Month.ToString();

            if (m.Length == 1)
            {
                m = "0" + m;
            }
            return dt.Day.ToString() + m + dt.Year.ToString();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "FirstName", "FirstName");
            ViewBag.bra = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SermonViewModel sv = sb.GetbyID(Convert.ToInt32(id));
            if (sv == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "FirstName", "FirstName");
            ViewBag.bra = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            return View(sv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SermonViewModel model)
        {
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "FirstName", "FirstName");
            ViewBag.bra = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            try
            {
                sb.UpdateSermon(model);
                return RedirectToAction("Index");
            }
            catch (Exception m)
            {
                Response.Write(m.Message);
            }

            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "FirstName", "FirstName");
            ViewBag.bra = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            return View(model);

        }
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SermonViewModel sv = sb.GetbyID(Convert.ToInt32(id));
            if (sv == null)
            {
                return HttpNotFound();
            }
            return View(sv);
        }

        public ActionResult Decline(int id)
        {
            try
            {
                SermonViewModel serm = sb.GetbyID(id);
                if (serm != null)
                {
                    sb.DeleteSermon(serm.ID);
                    return RedirectToAction("Index");
                }
            }
            catch(Exception v)
            {

            }
            return View("Index");
        }

        public ActionResult Approve(int id)
        {
            string feed = null;
            try
            {
                SermonViewModel serm = sb.GetbyID(id);
                BranchViewModel bran = bb.GetbyID(serm.branchID);
                string First = serm.pastorName.Substring(0, serm.pastorName.IndexOf(" "));
                string last = serm.pastorName.Substring(serm.pastorName.IndexOf(" ") + 1);
                var searchP = pb.GetAllPastors().Where(a => a.FirstName == First && a.Surname.Equals(last)).FirstOrDefault();

                if (serm != null)
                {
                    FinSermonViewModel checkserm = fb.GetAllFinalSermons().Find(x => x.pastorName == serm.pastorName && x.Date == serm.Date);
                    if (checkserm == null)
                    {
                        string startdate = serm.Date.ToShortDateString() + " " + serm.StartTime.ToShortTimeString();
                        string endDate = serm.Date.ToShortDateString() + " " + serm.EndTime.ToShortTimeString();

                        FinSermonViewModel finserm = new FinSermonViewModel
                        {
                            branchID = serm.branchID,
                            Date = serm.Date,
                            content = serm.content,
                            StartTime = serm.StartTime,
                            EndTime = serm.EndTime,
                            pastorName = serm.pastorName,
                            Description = "Sermon will be delivered by Pastor " + serm.pastorName + " at " + serm.BranchName + " branch",
                            EndDate = Convert.ToDateTime(endDate),
                            StartDate = Convert.ToDateTime(startdate)
                        };

                        fb.AddFinalSermon(finserm);
                        sb.DeleteSermon(id);
                        feed = " Sermon is Approved ";
                        sms.sending_full_sms(searchP.contact, "Hi Pastor " + serm.pastorName + ", you have been asked to deliver a sermon on the " + SermonDate(serm.Date) + 
                            " at " + serm.StartTime.ToShortTimeString() + " to " + serm.EndTime.ToShortTimeString()+" at "+ bran.BranchName+" branch and choose "+serm.content+".");
                        return RedirectToAction("Index", new { feed = feed });
                    }
                    else
                    {
                        feed = "Pastor " + serm.pastorName + " will be delivering a Sermon on " + serm.Date + " at another branch, choose another pastor";
                        return RedirectToAction("Index", new { feed = feed });
                    }

                }
                else
                {
                    ViewBag.feed = "Sermon is not approved";
                }
            }
            catch (Exception v)
            {
                ViewBag.feed = v.Message;
            }
            return View("Index", new { feed = feed });
        }

        public string SermonDate(DateTime date)
        {
            string findate = null;
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            if (day == 1)
            {
                findate += day + "st of ";
            }
            else if (day == 2)
            {
                findate += day + "nd of ";
            }
            else if (day == 3)
            {
                findate += day + "rd of ";
            }
            else if(day>3)
            {
                findate += day + "th of ";
            }

            if(month==1)
            {
                findate += "January ";
            }
            else if(month ==2)
            {
                findate += "February ";
            }
            else if (month == 3)
            {
                findate += "March ";
            }
            else if (month == 4)
            {
                findate += "April ";
            }
            else if (month == 5)
            {
                findate += "May ";
            }
            else if (month == 6)
            {
                findate += "June ";
            }
            else if (month == 7)
            {
                findate += "july ";
            }
            else if (month == 8)
            {
                findate += "August ";
            }
            else if (month == 9)
            {
                findate += "September ";
            }
            else if (month == 10)
            {
                findate += "October ";
            }
            else if (month ==11)
            {
                findate += "November ";
            }
            else if (month == 12)
            {
                findate += "December ";
            }
            return findate+year;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "FirstName", "FirstName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SermonViewModel sv = sb.GetbyID(Convert.ToInt32(id));
            if (sv == null)
            {
                return HttpNotFound();
            }
            return View(sv);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "FirstName", "FirstName");

            sb.DeleteSermon(id);
            return RedirectToAction("Index");
        }
    }
}