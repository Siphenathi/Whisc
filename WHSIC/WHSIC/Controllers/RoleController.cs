using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        private PositionBusiness pb = new PositionBusiness();
        private EmployeeBusiness eb = new EmployeeBusiness();
        RandomPassword rp = new RandomPassword();
        SMS sms = new SMS();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
        
        public string AdminContact()
        {
            string con = null;
            foreach(var p in _db.Users.ToList())
            {
                if (UserManager.IsInRole(p.Id, "Admin") && p.PhoneNumber!=null)
                {
                    con = p.PhoneNumber;
                }
            }
            return con;
        }

        public string TreasureContact()
        {
            string con = null;
            foreach (var p in _db.Users.ToList())
            {
                if (UserManager.IsInRole(p.Id, "Treasure") && p.PhoneNumber != null)
                {
                    con = p.PhoneNumber;
                }
            }
            return con;
        }

        public List<EmployeeViewModel> Namelist(List<EmployeeViewModel> list)
        {
            List<EmployeeViewModel> mylist = new List<EmployeeViewModel>();

            foreach (EmployeeViewModel p in list)
            {
                EmployeeViewModel pa = new EmployeeViewModel
                {
                    FirstName = p.FirstName + " " + p.LastName,
                    Email =p.Email
                };

                mylist.Add(pa);
            }
            return mylist;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            return View(RoleManager.Roles);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AssignRoles()
        {
            ViewBag.feed = null;
           var RoleManagerz = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            ViewBag.Employee = new SelectList(Namelist(eb.GetAllEmployee()), "Email", "FirstName");
            ViewBag.Role = new SelectList(RoleManagerz.Roles, "Name", "Name");
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignRoles(RolesViewModel model)
        {
            var RoleManagerz = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            ViewBag.Employee = new SelectList(Namelist(eb.GetAllEmployee()), "Email", "FirstName");
            ViewBag.Role = new SelectList(RoleManagerz.Roles, "Name", "Name");
            try
            {
                var _user = _db.Users.ToList().Find(x => x.UserName == model.Employee);
                string pass = GeneratePassword();
                EmployeeViewModel ep = (from e in eb.GetAllEmployee() where e.Email == model.Employee select e).ToList()[0];
                bool task = false;
                 task = await AssignToRole(_db, ep.Email, pass, ep.Contact, model.Role,ep.FirstName);
                if (task == true)
                {
                    if (_user == null)
                    {
                        ViewBag.feed = "User is assigned to the role successfully, password is " + pass + "  Check your email and confirm your account in order to log in."; 
                       //sms.sending_full_sms(ep.Contact, "Hi " + ep.FirstName + ", " + " you are given a Role to be the " + model.Role + " at Welcome Holy Spirit International Church" + " Your username: " + model.Employee + " Password : " + pass + "  , remember to change your password ");
                    }
                    else
                    {
                        ViewBag.feed = _user.UserName + " is assigned to the role successfully, This user must use its current password";
                    }

                }
                else
                {
                    ViewBag.feed = "Something went wrong, User is not assigned to the role check that the user is not in another Role";
                }
            }
            catch (Exception c)
            {

                throw;
            }
            ViewBag.Employee = new SelectList(Namelist(eb.GetAllEmployee()), "Email", "FirstName");
            ViewBag.Role = new SelectList(RoleManagerz.Roles, "Name", "Name");
            return View();
        }

       public async Task<bool> AssignToRole(WHSIC.Data.ApplicationDbContext context,string email,string password,string phone,string role,string name)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
            (new RoleStore<IdentityRole>(context));
            var um = new UserManager<ApplicationUser>
           (new UserStore<ApplicationUser>(context));

            var _user = context.Users.ToList().Find(x => x.UserName == email);

            ApplicationUser newuser = new ApplicationUser();

            if (_user == null)
            {
                newuser.UserName = email;
                newuser.PhoneNumber = phone;
                newuser.Email = email;

                ir = um.Create(newuser, password);
                if (ir.Succeeded == false)
                    return ir.Succeeded;

                ir = um.AddToRole(newuser.Id, role);
                await SendConfirmEmail(newuser, email,name,role,password);
                return ir.Succeeded;
            }
            ir = um.AddToRole(_user.Id, role);
            return ir.Succeeded;
        }

        public async Task SendConfirmEmail(ApplicationUser user,string email,string name,string role,string pass)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

            var myMessage = new SendGridMessage {From = new MailAddress("debuggers@outlook.com", "WHSIC")};

            myMessage.AddTo(email);

            string subject = "Role";

            string html = "<table style=\"border: none; font-family: verdana, tahoma, sans-serif;\">" +
                          "<tr> " +
                              "<td> <h3>Hello "+name+ ", </h3> <p>You are given a Role at Welcome Holy Spirit International Church to be the " + role + " your username is your email address and this is your password <strong style=\"color: blue;\">" + pass + "</strong> remember to change it once you login at WHSIC website. In order for you to login confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a> </p>" +
                              "<p>Regards,<br/>  Welcome holy Spirit International Church - Communications Team</p> </td> </tr> </table>";

            myMessage.Subject = subject;
            myMessage.Html = html;

 
            var transportWeb = new Web("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");
            // Send the email.
            await transportWeb.DeliverAsync(myMessage);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] RoleViewModel roleViewModel)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));

            if (ModelState.IsValid)
            {
                IdentityResult result = null;

                if (!RoleManager.RoleExists(roleViewModel.Name))
                {
                    result = await RoleManager.CreateAsync(new IdentityRole(roleViewModel.Name));
                }
                else
                {
                    ModelState.AddModelError("", "Role already exists.");
                }

                try
                {
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Role not Saved.");
                }

            }

            return View(roleViewModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(string id)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
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
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteRolesAsync()
        {
           ViewBag.feed = null;
           ViewBag.Employee = new SelectList(Namelist(eb.GetAllEmployee()), "Email", "FirstName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRolesAsync(RemoveFromRole model)
        {
            ViewBag.Employee = new SelectList(Namelist(eb.GetAllEmployee()), "Email", "FirstName");
            string feed = "something went wrong, User is not Removed in Role. check that the user is role bofore attempt to Remove";
            var RoleManagerz = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_db));

            try
            {
                var user = _db.Users.ToList().Find(x => x.UserName == model.Employee);

                if (user.Id != null)
                {
                    foreach (var roleName in RoleManagerz.Roles)
                    {
                        IdentityResult deletionResult = await UserManager.RemoveFromRoleAsync(user.Id, roleName.Name);
                        feed = "User is Removed from the Role";
                    }
                    ViewBag.feed = feed;
                }
                else
                {
                    ViewBag.feed = "The user you are trying to Remove from Role was not assigned in the Role";
                }
            }
            catch (Exception c)
            {

                ViewBag.feed = "The user you are trying to Remove from Role was not assigned in the Role";
            }
            
            ViewBag.Employee = new SelectList(Namelist(eb.GetAllEmployee()), "Email", "FirstName");
            return View(model);
        }




        public async Task SendContactEmail(string Names, string email, string PhoneNumber, string comment)
        {

            var myMessage = new SendGridMessage {From = new MailAddress(email, "WHSIC")};

            myMessage.AddTo("spantshwa95@gmail.com");

            string subject = "Role";

            string html = "<table style=\"border: none; font-family: verdana, tahoma, sans-serif;\">" +
                          "<tr> " +
                              "<td> <h3>Hello Secretary, " +" </h3><p> "+comment+"</p>" +"<br/>"+"<h3> From "+Names+"<br>"+"   Contact:  "+PhoneNumber+" </h3>"+
                              "<p>Regards,<br/>  Welcome holy Spirit International Church - Communications Team</p> </td> </tr> </table>";

            myMessage.Subject = subject;
            myMessage.Html = html;


            var transportWeb = new Web("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");
            // Send the email.
            await transportWeb.DeliverAsync(myMessage);
        }
    }
}