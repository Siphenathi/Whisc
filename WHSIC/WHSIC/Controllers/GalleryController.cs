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
    public class GalleryController : Controller
    {
        GalleryBusiness gb =new GalleryBusiness();
        public ActionResult Index(int? page)
        {
            const int pageSize = 16;
            int pageNo = (page ?? 1);

            var list = gb.GetAllImages().OrderByDescending(x=>x.GalleryId);
            return View(list.ToPagedList(pageNo, pageSize));
        }
        [Authorize(Roles = "Secretary")]
        public ActionResult Create()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase File)
        {
            try
            {
                GalleryViewModel gallery = new GalleryViewModel();

                byte[] data = new byte[File.ContentLength];
                File.InputStream.Read(data, 0, File.ContentLength);

                gallery.ImageData = data;

                gb.AddImage(gallery);
                ViewBag.feed = "Image is to gallery uploaded successfully";
            }
            catch (Exception v)
            {
                Response.Write(v.Message);
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            GalleryViewModel g = gb.GetbyID(id);
            return View(g);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                gb.DeleteImage(id);
                return RedirectToAction("Index");
            }
            catch (Exception b)
            {

            }
            return View("Index");
        }

    }
}