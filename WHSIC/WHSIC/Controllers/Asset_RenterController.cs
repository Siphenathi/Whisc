
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using WHSIC.Data;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class Asset_RenterController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        GenerateCode gc = new GenerateCode();
        SMS sms = new SMS();
        public ActionResult Index()
        {
            return View(db.Asset_Renters.ToList());
        }
        public FileStreamResult Showfile(int?id)
        {
            Stream output = new MemoryStream(getContract((int)id).Contract);
            return new FileStreamResult(output,"application/pdf");
        }
        public Asset_Renter getContract(int id)
        {
            return db.Asset_Renters.Find(id);
        }
        public ActionResult Create()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Asset_RenterViewModel model, HttpPostedFileBase file)
        {
            try
            {

                byte[] fileContents = null;
                fileContents = readFileContents(file);

                string code = GenerateCode();
                Asset_Renter renter = new Asset_Renter
                {
                    email = model.email,
                    Contact = model.Contact,
                    FullName = model.FullName,
                    Physical_Address = model.Physical_Address,
                    Code = code,
                    Contract = fileContents
                };
                db.Asset_Renters.Add(renter);
                db.SaveChanges();
                sms.sending_full_sms(model.Contact, "Hi " + model.FullName + ", your rental asset verification code is " + code);
                return RedirectToAction("VerificationCode", new { email = model.email,_code=code});
            }
            catch(Exception v)
            {

            }
            return View(model);
        }

        private byte[] readFileContents(HttpPostedFileBase file)
        {
            Stream fileStream = file.InputStream;
            var mStreamer = new MemoryStream();
            mStreamer.SetLength(fileStream.Length);
            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
            mStreamer.Seek(0, SeekOrigin.Begin);
            byte[] fileBytes = mStreamer.GetBuffer();
            return fileBytes;
        }

        public ActionResult VerificationCode(string email,string _code)
        {
            ViewBag.feed = null;
            ViewBag.email = email;
            ViewBag.code = _code;
            return View();
        }
        [HttpPost]
        public ActionResult VerificationCode(FormCollection form, CodeModel model)
        {
            string feed = "email not found";
            string email = form["txtemail"];
            try
            {
                Asset_Renter Renter = db.Asset_Renters.ToList().Find(x => x.email == email);
                if(Renter.Code.Equals(model.Code))
                {
                    return RedirectToAction("Index","Asset_Renter");
                }
                else
                {
                    feed = "Verification code is incorrect..!";
                   
                }
            }
            catch(Exception v)
            {

            }
            ViewBag.email = email;
            ViewBag.feed = feed;
            return View(model);
        }

        public ActionResult Edit(int?id)
        {
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset_Renter asse = db.Asset_Renters.Find(id);
            if (asse == null)
            {
                return HttpNotFound();
            }
            return View(asse);
        }
        [HttpPost]
        public ActionResult Edit(Asset_Renter model, HttpPostedFileBase file)
        {
            string feed = null;
            byte[] fileContents = null;
            fileContents = readFileContents(file);
            try
            {
                Asset_Renter renter = db.Asset_Renters.Find(model.Renter_code);
                renter.FullName = model.FullName;
                renter.email = model.email;
                renter.Contact = model.Contact;
                renter.Physical_Address = model.Physical_Address;
                renter.Contract = fileContents;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception v)
            {
                feed += v.Message;
            }
            ViewBag.feed = feed;
            return View(model);
        }
        public ActionResult ChangeContact(int? id)
        {
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset_Renter asse = db.Asset_Renters.Find(id);
            if (asse == null)
            {
                return HttpNotFound();
            }
            return View(asse);
        }
        [HttpPost]
        public ActionResult ChangeContact(Asset_Renter model)
        {
            try
            {
                string code = GenerateCode();
                Asset_Renter asse = db.Asset_Renters.Find(model.Renter_code);
                asse.Contact = model.Contact;
                asse.Code = code;
                db.SaveChanges();
                sms.sending_full_sms(model.Contact, "Hi " + model.FullName + ", your rental asset verification code is " + code);
                return RedirectToAction("VerificationCode", new { email = model.email, _code = code });
            }
            catch(Exception v)
            {

            }
            return View(model);
        }
        public string GenerateCode()
        {
            string pass = "";
            for (int i = 0; i < 100; i++)
            {
               pass = gc.Generate(5, 6);
            }
            return pass;
        }
    }
}