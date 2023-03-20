using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class DonationTypeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        DonationTypeBusiness dtb = new DonationTypeBusiness();
        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = dtb.GetAllDonationType();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Create()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        public ActionResult Create(DonationTypeViewModel model)
        {
            try
            {
                DonationTypeViewModel s = dtb.GetAllDonationType().Find(x => x.Description == model.Description);

                if (s == null)
                {
                    dtb.AddDonationType(model);
                    return RedirectToAction("Index");
                }
                ViewBag.m = " Description already exist";

            }
            catch (Exception d)
            {

            }
            return View();
        }
        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationTypeViewModel bv = dtb.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            return View(bv);
        }
        [HttpPost]
        public ActionResult Edit(DonationTypeViewModel model)
        {
            try
            {
                DonationTypeViewModel p = dtb.GetbyID(model.DonationtypeID);

                DonationType d = db.DonationType.ToList().Find(x => x.DonationtypeID == p.DonationtypeID);
                d.Description = model.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception c)
            {

            }
            return View(model);
        }
        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationTypeViewModel bv = dtb.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            return View(bv);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {

            try
            {
                dtb.DeleteDonationType(Convert.ToInt32(id));
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {

            }
            return View();
        }

    }
}