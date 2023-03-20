using WHSIC.Data;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;
using System.Threading.Tasks;
using SendGrid;
using Microsoft.AspNet.Identity;
using System.Net.Mail;

namespace WHSIC.Controllers
{
    public class HomeController : Controller
    {
        UploadBusiness pub = new UploadBusiness();
        ProgramBusiness pb = new ProgramBusiness();
        RoleController rc = new RoleController();


        public List<ProgramViewModel>List(List<ProgramViewModel>myList)
        {
            List<ProgramViewModel> newlist = new List<ProgramViewModel>();

            ProgramViewModel pv = new ProgramViewModel
            {
                PName = "All Programs"
            };
            newlist.Add(pv);

            foreach(ProgramViewModel pu in myList)
            {
                newlist.Add(pu);
            }

            return newlist;
        }


        public ActionResult Index(int? page)
        {
            const int pageSize = 8;
            int pageNo = (page ?? 1);
            ViewBag.m = new SelectList(List(pb.GetAllProgramies()), "PName", "PName");
            var list = pub.GetAllProgramUpload().OrderByDescending(p=>p.ProgramId);
            
            return View(list.ToPagedList(pageNo, pageSize));
        }
        [HttpPost]
        public async Task<ActionResult> Index(string name, string Names, string email, string PhoneNumber, string comment)
        {
            ViewBag.m = new SelectList(List(pb.GetAllProgramies()), "PName", "PName");

            const int pageSize = 8;
            int pageNo =  1;

            List<ProgramUploadViewModel> Search = new List<ProgramUploadViewModel>();
            if(name==null)
            {
                Search = (from s in pub.GetAllProgramUpload() select s).ToList();
            }
            else if (name== "All Programs")
            {
                Search = (from s in pub.GetAllProgramUpload() select s).ToList();
            }
            else
            {
                Search = (from s in pub.GetAllProgramUpload() where s.Name == name select s).ToList();
            }

            if (Names != null || email != null || PhoneNumber != null || comment != null)
            {

                ViewBag.message =await cont(Names, email, PhoneNumber, comment);
            }

            if(Search.Count==0)
            {
                ViewBag.message = "There is no uploaded event for the selected program";
            }
            return View(Search.ToPagedList(pageNo, pageSize));
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            
            ViewBag.message = "This is ur feedback";
            return View();
        }

        public async Task<string> cont(string Names,string email,string PhoneNumber,string comment)
        {
            string feed="";
            if (Names == null || email == null || PhoneNumber == null || comment == null)
            {
                feed = "Email not sent, please enter all required details";
                
            }
            else
            {
                await rc.SendContactEmail(Names, email, PhoneNumber, comment);
                feed = "Thanks for your interest, we will get back to you soon";
            }
            return feed;
        }
    }
}