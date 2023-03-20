using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class Asset_CategoryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Asset_Categories.ToList());
        }
        public ActionResult Create()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Asset_CategoryViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Asset_Category cat1 = db.Asset_Categories.ToList().Find(x => x.CName.Equals(model.CName));
                    if (cat1 == null)
                    {
                        Asset_Category cat = new Asset_Category
                        {
                            CName = model.CName,
                            Type = model.Type
                        };
                        db.Asset_Categories.Add(cat);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.feed = "The is already an Asset Category with the same name as " + model.CName;
                        return View(model);
                    }
                }
            }
            catch (Exception v)
            {
                ViewBag.feed = v.Message;
                return View(model);
            }


            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset_Category asset_Category = db.Asset_Categories.Find(id);
            if (asset_Category == null)
            {
                return HttpNotFound();
            }
            return View(asset_Category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asset_Category asset_Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asset_Category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asset_Category);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset_Category asset_Category = db.Asset_Categories.Find(id);
            if (asset_Category == null)
            {
                return HttpNotFound();
            }
            return View(asset_Category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset_Category asset_Category = db.Asset_Categories.Find(id);
            db.Asset_Categories.Remove(asset_Category);
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
