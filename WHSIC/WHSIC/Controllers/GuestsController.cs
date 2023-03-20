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
    public class GuestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Guests.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        public ActionResult Create()
        {
            ViewBag.feed = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Guest guest)
        {
            string feed = null;
            try
            {
                Guest g = db.Guests.ToList().Find(x => x.Guestemail == guest.Guestemail);
                if (g==null)
                {


                    db.Guests.Add(guest);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    feed = "Guest already exist or there is another guest with the same email address ";
                }
            }
            catch(Exception v)
            {

            }
            ViewBag.feed = feed;
            return View(guest);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guest guest)
        {

            string feed = null;
            try
            {
                    db.Entry(guest).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                //}
                //else
                //{
                //    feed = "Guest already exist or there's another guest with the same email address ";
                //}
            }
            catch (Exception v)
            {
                feed += v.Message;
            }
            ViewBag.feed = feed;
            return View(guest);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guest guest = db.Guests.Find(id);
            db.Guests.Remove(guest);
            db.SaveChanges();
            return RedirectToAction("Index");
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
