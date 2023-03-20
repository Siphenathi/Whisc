using WHSIC.Data;
using WHSIC.Model;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using WHSIC.BusinessLogic;

namespace WHSIC.Controllers
{
    public class PastorController : Controller
    {
        readonly ApplicationDbContext _db = new ApplicationDbContext();
        readonly PastorBusiness _pb = new PastorBusiness();

        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = _pb.GetAllPastors();
            return View(list.ToPagedList(pageNo, pageSize));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]


        public ActionResult AddPastor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPastor(PastorViewModel model, HttpPostedFileBase file)
        {
            try
            {
                string tt = "";
                string full = model.FirstName + " " + model.Surname;

                PastorViewModel pa = _pb.GetAllPastors().Find(x => x.FirstName.ToLower() == model.FirstName.ToLower() && x.Surname.ToLower() == model.Surname.ToLower());
                PastorViewModel em = _pb.GetAllPastors().Find(x => x.email == model.email);

                if (pa != null || em != null)
                {
                    ViewBag.m = " Pastor already exist or there's another Pastor using this email";
                }
                else
                {
                   
                    if (file != null)
                    {
                        byte[] data = null;
                        data = new byte[file.ContentLength];
                        file.InputStream.Read(data, 0, file.ContentLength);

                        model.image = data;
                        _pb.AddPastor(model);

                        return RedirectToAction("Index");
                    }
                    else
                    { 
                         Image img = Image.FromFile(Server.MapPath("~/Images/DefaultPastor.jpg"));
                         byte[] dat = ImageToByte(img);

                        model.image = dat;
                        _pb.AddPastor(model);

                        return RedirectToAction("Index");
                    }

                }
            }

            catch (Exception n)
            {
                //ViewBag.m = n.Message;
                Response.Write(n.Message);
            }

            return View(model);
        }

        //public byte[] DefaultImage()
        //{
        //    Image img = Image.FromFile(Server.MapPath("~/Images/DefaultPastor.jpg"));
        //    byte[] dat = ImageToByte(img);
        //    return dat;
        //}
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] xbyte = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return xbyte;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastorViewModel pa = _pb.GetbyID(Convert.ToInt32(id));
            if (pa == null)
            {
                return HttpNotFound();
            }
            return View(pa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PastorViewModel model, HttpPostedFileBase File)
        {
            try
            {

                byte[] data = new byte[File.ContentLength];
                File.InputStream.Read(data, 0, File.ContentLength);

                model.image = data;
                _pb.UpdatePastor(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception m)
            {
                Response.Write(m.Message);
            }
            return View(model);
        }
        public ActionResult Details(int? id)
        {
            List<PastorViewModel> pa = new List<PastorViewModel>();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             pa = (from p in _pb.GetAllPastors() where p.pastorID==Convert.ToInt16(id) select p).ToList();
            if (pa == null)
            {
                return HttpNotFound();
            }
            
            return View(pa);
        }
       
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastorViewModel pa = _pb.GetbyID(Convert.ToInt32(id));
            if (pa == null)
            {
                return HttpNotFound();
            }
            return View(pa);
        }
        // POST: /Movies/Delete/5 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _pb.DeletePastor(id);
                return RedirectToAction("Index");
            }
            catch (Exception c)
            {

            }
            return View();
        }

        //if (File == null)
        //{
        //byte[] imgdata = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(""));
        //Image imgdefault = Image.FromFile("/Images/DefaultPastor.jpg");

        //ImageConverter _imageConverter = new ImageConverter();
        // data = (byte[])_imageConverter.ConvertTo(imgdefault, typeof(byte[]));


        //FileInfo fileInfo = new FileInfo("/Images/DefaultPastor.jpg");
        //long imageFileLength = fileInfo.Length;
        //FileStream fs = new FileStream("/Images/DefaultPastor.jpg", FileMode.Open, FileAccess.Read);
        //BinaryReader br = new BinaryReader(fs);
        //data = br.ReadBytes((int)imageFileLength);

    }
}
