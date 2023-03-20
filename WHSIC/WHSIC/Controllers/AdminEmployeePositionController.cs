using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class AdminEmployeePositionController : Controller
    {
        PositionBusiness pb = new PositionBusiness();
        public ActionResult GetAllPosition(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = pb.GetAllPositions();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public ActionResult AddPosition()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPosition(PositionViewModel model)
        {
            try
            {
                PositionViewModel s = pb.GetAllPositions().Find(x => x.Description == model.Description);

                if (s == null)
                {
                    pb.AddPositions(model);
                    return RedirectToAction("GetAllPosition");
                }
                ViewBag.m = " Description already exist";

            }
            catch (Exception d)
            {

            }
            return View();
        }

        public ActionResult UpdatePosition(int? id)
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
        public ActionResult UpdatePosition(PositionViewModel model)
        {
            try
            {
                pb.UpdatePositions(model);
                return RedirectToAction("GetAllPosition");
            }
            catch (Exception c)
            {

            }
            return View(model);
        }
        public ActionResult DeletePosition(int? id)
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
        public ActionResult DeletePosition(int id)
        {

            try
            {
                pb.DeletePositions(Convert.ToInt32(id));
                return RedirectToAction("GetAllPosition");
            }
            catch (Exception b)
            {

            }
            return View();
        }
    }
}