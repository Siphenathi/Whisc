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
    public class AdminPartTimePositionController : Controller
    {
        PositionBusiness pb = new PositionBusiness();
        PartTimeBusiness pt = new PartTimeBusiness();
        public ActionResult GetAllPartTime(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = pt.GetAllPartTime();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public ActionResult AddPartTime()
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }

        [HttpPost]
        public ActionResult AddPartTime(PartTimeViewModel model)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            try
            {
                PositionViewModel p = pb.GetbyID(model.PositionID);
                model.Description = p.Description;

                pt.AddPartTime(model);
                return RedirectToAction("GetAllPartTime");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }
        public ActionResult DeletePartTime(int? id)
        {
            PartTimeViewModel ft = pt.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult DeletePartTime(int id)
        {
            try
            {
                pt.DeletePartTime(id);
                return RedirectToAction("GetAllPartTime");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

        public ActionResult UpdatePartTime(int? id)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            PartTimeViewModel ft = pt.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult UpdatePartTime(PartTimeViewModel model)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            try
            {
                pt.UpdatePartTime(model);
                return RedirectToAction("GetAllPartTime");
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