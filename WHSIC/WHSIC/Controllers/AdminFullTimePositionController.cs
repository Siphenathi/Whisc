using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class AdminFullTimePositionController : Controller
    {
        FullTimeBusiness fb = new FullTimeBusiness();
        PositionBusiness pb = new PositionBusiness();
        public ActionResult GetAllFullTime(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = fb.GetAllFullTime();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public ActionResult AddFullTime()
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }

        [HttpPost]
        public ActionResult AddFullTime(FullTimeViewModel model)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            try
            {
                PositionViewModel p = pb.GetbyID(model.PositionID);
                model.Description = p.Description;

                fb.AddFullTime(model);
                return RedirectToAction("GetAllFullTime");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }
        public ActionResult DeleteFullTime(int? id)
        {
            FullTimeViewModel ft = fb.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult DeleteFullTime(int id)
        {
            try
            {
                fb.DeleteFullTime(id);
                return RedirectToAction("GetAllFullTime");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

        public ActionResult UpdateFullTime(int? id)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            FullTimeViewModel ft = fb.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult UpdateFullTime(FullTimeViewModel model)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            try
            {
                fb.UpdateFullTime(model);
                return RedirectToAction("GetAllFullTime");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }
    }
}