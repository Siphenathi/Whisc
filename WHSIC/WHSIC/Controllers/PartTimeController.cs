using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class PartTimeController : Controller
    {
        PositionBusiness pb = new PositionBusiness();
        PartTimeBusiness pt = new PartTimeBusiness();
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = pt.GetAllPartTime();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        public ActionResult Create()
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }

        [HttpPost]
        public ActionResult Create(PartTimeViewModel model)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            try
            {
                PositionViewModel p = pb.GetbyID(model.PositionID);
                model.Description = p.Description;

                pt.AddPartTime(model);
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            return View();
        }

        public ActionResult Details(int id)
        {
            PartTimeViewModel ft = pt.GetbyID(id);
            return View(ft);
        }
        public ActionResult Delete(int? id)
        {
            PartTimeViewModel ft = pt.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                pt.DeletePartTime(id);
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            PartTimeViewModel ft = pt.GetbyID(Convert.ToInt32(id));
            return View(ft);
        }
        [HttpPost]
        public ActionResult Edit(PartTimeViewModel model)
        {
            ViewBag.PositionID = new SelectList(pb.GetAllPositions(), "PositionID", "Description");
            try
            {
                pt.UpdatePartTime(model);
                return RedirectToAction("Index");
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