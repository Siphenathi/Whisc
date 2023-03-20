using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class ExternalDonorController : Controller
    {

        ServiceeBusiness sb = new ServiceeBusiness();
        DonorBusiness _db = new DonorBusiness();
        DonationBusiness don = new DonationBusiness();
        DonorTypeBusiness typb = new DonorTypeBusiness();
        DonationTypeBusiness dt = new DonationTypeBusiness();
        InternetTime it = new InternetTime();
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult ChooseDonation()
        {
            return View();
        }
        [Authorize]
        public ActionResult MakeDonation()
        {
            DonorViewModel other = _db.GetAllDonor().Find(x=>x.Name== "Other");
            ViewBag.Donationtype = new SelectList(dt.GetAllDonationType(), "DonationtypeID", "Description");
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        public ActionResult MakeDonation(DonationViewModel model,FormCollection form)
        {
            ViewBag.Donationtype = new SelectList(dt.GetAllDonationType(), "DonationtypeID", "Description");
            string feed = null;
            try
            {
                DonorTypeViewModel dontype = typb.GetAllDonarType().Find(x => x.Description == "Other");
                string email = User.Identity.Name;

                DonorViewModel findD = _db.GetAllDonor().Find(x => x.Email == email);

                if(findD==null)
                {
                    DonorViewModel newdon = new DonorViewModel
                    {
                        DonortypeID = dontype.DonortypeID,
                        Name = email,
                        Email = email,
                        Contact = "N/A",
                        Address = "N/A",
                        tax_no = "N/A",
                        Image = DefaultImage()
                    };
                    _db.AddDonor(newdon);

                    DonorViewModel other = _db.GetAllDonor().Find(x => x.Email == email);
                    model.Date = it.GetNistTime();
                    model.Donor_ID = other.Donor_ID;
                    model.Payment_Method = "Online Payment";
                    don.AddDonation(model);
                    return RedirectToAction("ValidateCommand", "Payfast", new { cost = model.Amount });
                }
                else
                {
                    DonorViewModel other = _db.GetAllDonor().Find(x => x.Email == email);
                    model.Date = it.GetNistTime();
                    model.Donor_ID = findD.Donor_ID;
                    model.Payment_Method = "Online Payment";
                    don.AddDonation(model);
                    return RedirectToAction("ValidateCommand", "Payfast", new { cost = model.Amount });
                }
                
               
                

            }
            catch(Exception v)
            {
                feed = v.Message;
            }
            ViewBag.Donationtype = new SelectList(dt.GetAllDonationType(), "DonationtypeID", "Description");
            ViewBag.feed = feed;
            return View();
        }

        public byte[] DefaultImage()
        {
            Image img = Image.FromFile(Server.MapPath("~/Images/DefaultPastor.jpg"));
            byte[] dat = ImageToByte(img);
            return dat;
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] xbyte = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return xbyte;
        }
    }
}