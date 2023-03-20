using WHSIC.Data;
using PagedList;
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
    public class RejectedUploadController : Controller
    {
        // GET: RejectedUpload
        ApplicationDbContext db = new ApplicationDbContext();
        RejectedBusiness rb = new RejectedBusiness();
        ProgramBusiness pb = new ProgramBusiness();
        TempBusiness tb = new TempBusiness();

        [Authorize(Roles ="Secretary")]
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = rb.GetAllRejectedUpload();

            return View(list.ToPagedList(pageNo, pageSize));
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RejectedViewModel pc = rb.GetbyID(Convert.ToInt32(id));

            if (pc == null)
            {
                return HttpNotFound();
            }
            ViewBag.pc = new SelectList(pb.GetAllProgramies(), "PName", "PName");
            return View(pc);

        }
        [HttpPost]
        public ActionResult Edit(RejectedViewModel model, HttpPostedFileBase File)
        {
            ViewBag.pc = new SelectList(pb.GetAllProgramies(), "PName", "PName");
            try
            {

                byte[] data = new byte[File.ContentLength];
                File.InputStream.Read(data, 0, File.ContentLength);

                model.Image = data;

                tb.RestoreUpload(model);
                Delete(model.ProgramId);

                db.SaveChanges();

                return RedirectToAction("Index", "RejectedUpload");
            }
            catch (Exception b)
            {
                Response.Write(b.Message);
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            try
            {
                rb.DeleteRejectedUpload(id);
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