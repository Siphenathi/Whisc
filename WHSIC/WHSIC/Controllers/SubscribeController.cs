using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;

namespace WHSIC.Controllers
{
    public class SubscribeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        InternetTime it = new InternetTime();
        
        public ActionResult Index()
        {
            ViewBag.counts = db.Subscibers.ToList().Count;
            return View(db.Subscibers.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subscibers model)
        {

            string feed = null;
           try
            {
                Subscibers sub = db.Subscibers.ToList().Find(x => x.Email == model.Email);
                if(sub==null)
                {
                    db.Subscibers.Add(model);
                    db.SaveChanges();
                    feed= "We are looking foward to informing you";
                }
                else
                {
                    feed = "You have already subscribed, when we have vacancies we will notify you..";
                }
            }
            catch(Exception v)
            {
                feed += v.Message;
            }
            ViewBag.feed = feed;
            return View(model);
        }

        public ActionResult JobView()
        {
            if (db.Jobs.ToList().Count == 0)
            {
                return RedirectToAction("Create", "Subscribe");
            }
            ViewBag.JobsAvaillable = db.Jobs.ToList().Count();
            foreach (var item in db.Jobs.ToList())
            {
                string sdate = it.GetNistTime().ToShortDateString();
                string cdate = item.ClosingDate.ToShortDateString();
                if (Convert.ToDateTime(cdate) < Convert.ToDateTime(sdate))
                {
                    db.Jobs.Remove(item);
                    db.SaveChanges();
                }
            }
            return View(db.Jobs.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
