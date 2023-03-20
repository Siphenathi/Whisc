using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class BranchController : Controller
    {
        BranchBusiness bb = new BranchBusiness();
        PastorBusiness pb = new PastorBusiness();

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

        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = bb.GetAllBranches();
            return View(list.ToPagedList(pageNo, pageSize));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.m = null;
            ViewBag.pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BranchViewModel model)
        {
            ViewBag.pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            try
            {
                BranchViewModel ba = bb.GetAllBranches().Find(x => x.BranchName.ToLower() == model.BranchName.ToLower());
                BranchViewModel bap = bb.GetAllBranches().Find(x => x.pastorID == model.pastorID);
                PastorViewModel pastor = pb.GetbyID(model.pastorID);
                if (ba != null)
                {
                    ViewBag.m = " Branch Already exist!";
                    return View(model);
                }
                else
                {
                    if (bap != null)
                    {
                        ViewBag.m = "Pastor "+pastor.FirstName + " " +pastor.Surname+" is already assigned to another branch!";
                    }
                    else
                    {


                        bb.AddBranch(model);
                        return RedirectToAction("Index");
                    }

                }

            }
            catch (Exception n)
            {
                Response.Write(n.Message);
            }
            ViewBag.pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            return View(model);

        }




        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            ViewBag.pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchViewModel bv = bb.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            ViewBag.pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            return View(bv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BranchViewModel model)
        {
            ViewBag.pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            try
            {
                bb.UpdateBranch(model);
                return RedirectToAction("Index");
            }
            catch (Exception m)
            {
                ViewBag.m = m.Message;
            }
            ViewBag.pastorlist = new SelectList(namelist(pb.GetAllPastors()), "pastorID", "FirstName");
            return View(model);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchViewModel bv = bb.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            return View(bv);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchViewModel bv = bb.GetbyID(Convert.ToInt32(id));
            if (bv == null)
            {
                return HttpNotFound();
            }
            return View(bv);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            bb.DeleteBranch(id);
            return RedirectToAction("Index");
        }
    }
}