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
    public class MemberController : Controller
    {
        MemberBusiness mb = new MemberBusiness();
        BranchBusiness bb = new BranchBusiness();

        public JsonResult GetEmployees(string term)
        {
            List<string> employee = mb.GetAllMembers().Where(x => x.email.StartsWith(term))
                .Select(y => y.email).ToList();
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Index(int?page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = mb.GetAllMembers();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchTerm, int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            List<MemberViewModel> Member;

            if (string.IsNullOrEmpty(searchTerm))
            {
                Member = mb.GetAllMembers();
            }
            else
            {
                Member = mb.GetAllMembers().Where(x => x.email.StartsWith(searchTerm)).ToList();
            }
            var list = Member;
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Create()
        {
            ViewBag.branch = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberViewModel model)
        {
            ViewBag.branch = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");

            if (ModelState.IsValid)
            {
                try
                {
                    mb.AddMembers(model);
                    return RedirectToAction("Index");
                }
                catch (Exception d)
                {
                    ViewBag.feed = d.Message;
                }
                ViewBag.branch = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            }
                return View();
        }
        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Edit(string id)
        {
            ViewBag.branch = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            MemberViewModel mv = mb.GetbyID(id);
            return View(mv);
        }
        [HttpPost]
        public ActionResult Edit(MemberViewModel model)
        {
            ViewBag.branch = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            try
            {
                mb.UpdateMembers(model);
                return RedirectToAction("Index");
            }
            catch(Exception f)
            {
                Response.Write(f.Message);
            }
            ViewBag.branch = new SelectList(bb.GetAllBranches(), "BranchID", "BranchName");
            return View();
        }

        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Delete(string id)
        {
           
            MemberViewModel mv = mb.GetbyID(id);
            return View(mv);
        }
        [HttpPost]
        public ActionResult Delete(string id,string x)
        {
            try
            {
                mb.DeleteMembers(id);
                return RedirectToAction("Index");
            }
            catch (Exception f)
            {
                Response.Write(f.Message);
            }

            return View();
        }
        public ActionResult Details(string id)
        {
            MemberViewModel mv = mb.GetbyID(id);
            return View(mv);
        }


    }
}