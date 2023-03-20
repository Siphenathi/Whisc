using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;


namespace WHSIC.Controllers
{
    public class DonorController : Controller
    {
        //ApplicationDbContext db = new ApplicationDbContext();
        DonorBusiness dtb = new DonorBusiness();
        DonorTypeBusiness dt = new DonorTypeBusiness();
        RandomPassword rp = new RandomPassword();
        SMS sms = new SMS();

        private ApplicationUserManager _userManager;
        public DonorController()
        {

        }

        public DonorController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public JsonResult GetEmployees(string term)
        {
            List<string> employee = dtb.GetAllDonor().Where(x => x.Email.ToUpper().StartsWith(term.ToUpper())).Select(y => y.Email).ToList();
            if (employee.Count==0)
            {
              employee=dtb.GetAllDonor().Where(x=>x.Name.ToUpper().StartsWith(term.ToUpper())).Select(y=>y.Name).ToList();  
            }
            return Json(employee, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Index(int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            var list = dtb.GetAllDonor();
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchTerm, int? page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);

            List<DonorViewModel> Program;

            if (string.IsNullOrEmpty(searchTerm))
            {
                Program = dtb.GetAllDonor();
            }
            else
            {
                Program = dtb.GetAllDonor().Where(x => x.Email.StartsWith(searchTerm)).ToList();
            }
            var list = Program;
            return View(list.ToPagedList(pageNo, pageSize));
        }

        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Create()
        {
            ViewBag.Donortype = new SelectList(dt.GetAllDonarType(), "DonortypeID", "Description");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(DonorViewModel model, HttpPostedFileBase File)
        {
           
            ViewBag.Donortype = new SelectList(dt.GetAllDonarType(), "DonortypeID", "Description");
            try
            {
                DonorViewModel d = dtb.GetAllDonor().Find(x=>x.Name==model.Name || x.Email==model.Email);
                if (d == null)
                {
                    string pass = GeneratePassword();
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    var result = await UserManager.CreateAsync(user,pass );

                    if (result.Succeeded)
                    {
                        if (model.tax_no == null)
                            model.tax_no = "Not Applicable";
                        
                        if(File != null)
                        {
                            byte[] data = null;
                            data = new byte[File.ContentLength];
                            File.InputStream.Read(data, 0, File.ContentLength);

                            model.Image = data;

                            dtb.AddDonor(model);

                            DonorViewModel don = dtb.GetAllDonor().Find(x => x.Email == model.Email);

                            await SendEmailConfirm(user,don.Name,pass,don.Description);
                            return RedirectToAction("Index");
                        }
                        else if(File==null)
                        {
                            Image img = Image.FromFile(Server.MapPath("~/Images/DefaultPastor.jpg"));
                            byte[] dat = ImageToByte(img);

                            model.Image = dat;
                            dtb.AddDonor(model);

                            DonorViewModel don = dtb.GetAllDonor().Find(x => x.Email == model.Email);

                            await SendEmailConfirm(user, don.Name, pass,don.Description);

                            return RedirectToAction("Index");
                        }
                        
                    }
                    else
                        ViewBag.m = "Donor is not Added,it may happen that this Donor already exist" ;
                }
                else
                ViewBag.m = "Donor already exist";
            }
            catch(Exception c)
            {

            }
            ViewBag.Donortype = new SelectList(dt.GetAllDonarType(), "DonortypeID", "Description");
            return View(model);
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] xbyte = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return xbyte;
        }


        public async Task SendEmailConfirm(ApplicationUser user,string name,string pass,string donortype)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var myMessage = new SendGridMessage();

            myMessage.From = new MailAddress("debuggers@outlook.com", "WHSIC");

            myMessage.AddTo(user.Email);

            string subject = "Registration";

            string html = "<table style=\"border: none; font-family: verdana, tahoma, sans-serif;\">" +
                          "<tr> " +
                              "<td> <h3>Hi " + name + ", </h3><p> You are successfully registered at WHSIC as " + donortype + " your Username is " + user.Email + " and your password is <strong style=\"color: blue;\">" + pass + "</strong> change this password after you have confirm your account. You confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>" +
                              "<p>Regards,<br/> Welcome holy Spirit International Church - Communications Team</p> </td> </tr> </table>";

            myMessage.Subject = subject;
            myMessage.Html = html;


            var transportWeb = new Web("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");
            // Send the email.
            await transportWeb.DeliverAsync(myMessage);
        }

        public string GeneratePassword()
        {
            string pass = "";
            for (int i = 0; i < 100; i++)
            {
                pass = rp.Generate(8, 10);
            }
            return pass;
        }
        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Details(int? id)
        {
            DonorViewModel d = dtb.GetbyID(Convert.ToInt32(id));
            return View(d);
        }
        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Delete(int?id)
        {
            DonorViewModel d = dtb.GetbyID(Convert.ToInt32(id));
            return View(d);
        }
        [HttpPost]

        public ActionResult Delete(int id)
        {
            try
            {
                dtb.DeleteDonor(id);
                return RedirectToAction("Index");
            }
            catch(Exception p)
            {

            }
            return View();
        }
        [Authorize(Roles = "Secretary, Admin")]
        public ActionResult Edit(int id)
         {
            ViewBag.Donortype = new SelectList(dt.GetAllDonarType(), "DonortypeID", "Description");
            DonorViewModel d = dtb.GetbyID(Convert.ToInt32(id));
            return View(d);
        }
        [HttpPost]
        public ActionResult Edit(DonorViewModel model, HttpPostedFileBase File)
        {
            ViewBag.Donortype = new SelectList(dt.GetAllDonarType(), "DonortypeID", "Description");
            try
            {
                byte[] data = null;
                data = new byte[File.ContentLength];
                File.InputStream.Read(data, 0, File.ContentLength);

                model.Image = data;

                dtb.UpdateDonor(model);
                return RedirectToAction("Index");
            }
            catch(Exception c)
            {

            }
            ViewBag.Donortype = new SelectList(dt.GetAllDonarType(), "DonortypeID", "Description");
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}