using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class AvailabilityController : Controller
    {
        private AvailabililtyBusiness ab = new AvailabililtyBusiness();
        private PastorBusiness pb = new PastorBusiness();


        public List<PastorViewModel> namelist(List<PastorViewModel> list)
        {
            List<PastorViewModel> mylist = new List<PastorViewModel>();

            foreach (PastorViewModel p in list)
            {
                PastorViewModel pa = new PastorViewModel
                {
                    FirstName = p.FirstName + " " + p.Surname,
                    pastorID = p.pastorID
                };

                mylist.Add(pa);
            }
            return mylist;
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult Index()
        {
            return View(ab.GetAllAvailability());
        }
        [Authorize(Roles = "Secretary,Admin")]
        public ActionResult Create()
        {
            ViewBag.m = null;
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            return View();
        }
        [HttpPost]
        public ActionResult Create(AvailabilityViewModel model)
        {
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            try
            {
                ab.AddAvailability(model);
                ViewBag.m = "Dates are saved";
            }
            catch (Exception c)
            {

                Response.Write(c.Message);
            }
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            return View();
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityViewModel bv = ab.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            return View(bv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AvailabilityViewModel model)
        {
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            try
            {
                ab.UpdateAvailability(model);
                return RedirectToAction("Index");
            }
            catch (Exception m)
            {
                Response.Write(m.Message);
            }
            ViewBag.Pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            return View(model);
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AvailabilityViewModel bv = ab.GetbyID(Convert.ToInt32(id));
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
                ab.DeleteAvailability(Convert.ToInt32(id));
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {

            }
            return View();
        }
    }
}