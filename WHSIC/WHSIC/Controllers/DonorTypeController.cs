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
    public class DonorTypeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        DonorTypeBusiness dtb = new DonorTypeBusiness();
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = dtb.GetAllDonarType();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DonorTypeViewModel model)
        {
            try
            {
                DonorTypeViewModel s = dtb.GetAllDonarType().Find(x => x.Description == model.Description);

                if (s == null)
                {
                    dtb.AddDonarType(model);
                    return RedirectToAction("Index");
                }
                ViewBag.m = " Description already exist";

            }
            catch (Exception d)
            {

            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonorTypeViewModel bv = dtb.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            return View(bv);
        }
        [HttpPost]
        public ActionResult Edit(DonorTypeViewModel model)
        {
            try
            {
                DonorTypeViewModel p = dtb.GetbyID(model.DonortypeID);

                DonorType d = db.DonorType.ToList().Find(x => x.DonortypeID == p.DonortypeID);
                d.Description = model.Description;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception c)
            {

            }
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonorTypeViewModel bv = dtb.GetbyID(Convert.ToInt32(id));
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
                dtb.DeleteDonarType(Convert.ToInt32(id));
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {

            }
            return View();
        }
    }
}