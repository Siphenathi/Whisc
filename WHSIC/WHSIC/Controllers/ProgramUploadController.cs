using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;
using PagedList;
using System.Net;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class ProgramUploadController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UploadBusiness ub = new UploadBusiness();
        ProgramBusiness pb = new ProgramBusiness();


        public List<ProgramViewModel> List(List<ProgramViewModel> myList)
        {
            List<ProgramViewModel> newlist = new List<ProgramViewModel>();

            ProgramViewModel pv = new ProgramViewModel
            {
                PName = "All Programs"
            };
            newlist.Add(pv);

            foreach (ProgramViewModel pu in myList)
            {
                newlist.Add(pu);
            }

            return newlist;
        }


        [Authorize(Roles ="Secretary")]
        public ActionResult Index(int? page)
        {
            const int pageSize = 8;
            int pageNo = (page ?? 1);
            ViewBag.m = new SelectList(List(pb.GetAllProgramies()), "PName", "PName");
            var list = ub.GetAllProgramUpload();

            return View(list.ToPagedList(pageNo, pageSize));
        }
        [HttpPost]
        public ActionResult Index(string  name)
        {
            ViewBag.m = new SelectList(List(pb.GetAllProgramies()), "PName", "PName");

            const int pageSize = 8;
            int pageNo = 1;

            List<ProgramUploadViewModel> Search = new List<ProgramUploadViewModel>();

            if (name == "All Programs")
            {
                Search = (from s in ub.GetAllProgramUpload() select s).ToList();
            }
            else
            {
                Search = (from s in ub.GetAllProgramUpload() where s.Name == name select s).ToList();
            }
            return View(Search.ToPagedList(pageNo, pageSize));
        }


        [HttpGet]
        [Authorize(Roles ="Secretary")]
        public ActionResult Create(string f)
        {
            ModelState.Clear();
            ViewBag.pc = new SelectList(pb.GetAllProgramies(), "PName", "PName");
            ViewBag.feed = f;
            
            return View();
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramUploadViewModel pu = ub.GetbyID(Convert.ToInt32(id));

            if (pu == null)
            {
                return HttpNotFound();
            }
            ViewBag.pc = new SelectList(pb.GetAllProgramies(), "PName", "PName");
            return View(pu);

        }

        [HttpPost]
        public ActionResult Edit(ProgramUploadViewModel model, HttpPostedFileBase File)
        {
            ViewBag.pc = new SelectList(pb.GetAllProgramies(), "PName", "PName");
            try
            {
                byte[] data = new byte[File.ContentLength];
                File.InputStream.Read(data, 0, File.ContentLength);

                model.Image = data;
                ub.UpdateProgramUpload(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

        public ActionResult ViewOneP(int? id)
        {
            ViewBag.m = id.ToString();
            return View(ub.GetAllProgramUpload());
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProgramUploadViewModel pu = ub.GetbyID(Convert.ToInt32(id));

            if (pu == null)
            {
                return HttpNotFound();
            }
            return View(pu);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                    ub.DeleteProgramUpload(id);
                    return RedirectToAction("Index");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

    }
}