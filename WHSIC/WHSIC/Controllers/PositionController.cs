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
    public class PositionController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        PositionBusiness pb = new PositionBusiness();
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = pb.GetAllPositions();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PositionViewModel model)
        {
            try
            {
                PositionViewModel s = pb.GetAllPositions().Find(x => x.Description == model.Description);

                if(s==null)
                {
                    pb.AddPositions(model);
                    return RedirectToAction("Index");
                }
                ViewBag.m = " Description already exist";
                
            }
            catch(Exception d)
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
            PositionViewModel bv = pb.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            return View(bv);
        }
        [HttpPost]
        public ActionResult Edit(PositionViewModel model)
        {
            try
            {
                Position p= new Position
                {
                    Description=model.Description
                };

                db.SaveChanges();
               // pb.UpdatePositions(model);
                return RedirectToAction("Index");
            }
            catch(Exception c)
            {

            }
            return View(model);
        }
        public ActionResult Delete (int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PositionViewModel bv = pb.GetbyID(Convert.ToInt32(id));
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
                pb.DeletePositions(Convert.ToInt32(id));
                return RedirectToAction("Index");
            }
            catch(Exception b)
            {

            }
            return View();
        }

    }
}