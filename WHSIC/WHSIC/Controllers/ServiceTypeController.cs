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
    public class ServiceTypeController : Controller
    {
        ServiceTypeBusiness stb = new ServiceTypeBusiness();
        [Authorize(Roles ="Treasure")]
        public ActionResult GetAllServiceType(int?page)
        {
            const int pageSize = 5;
            int pageNo = (page ?? 1);
            var List = (from s in stb.GetAllServiceType() orderby s.TypeID descending select s).ToList();
            return View(List.ToPagedList(pageNo, pageSize));
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult CreateServiceType()
        {
            ViewBag.m = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateServiceType(ServiceTypeViewModel model)
        {
            try
            {
                ServiceTypeViewModel type = stb.GetAllServiceType().Find(x => x.Description == model.Description.Trim());

                if(type==null)
                {
                    model.dismiss = true;
                    stb.AddServiceType(model);
                    return RedirectToAction("GetAllServiceType");
                }
                else
                {
                    ViewBag.m = "Service Type called " + model.Description+" already exist";
                }
                
            }
            catch(Exception v)
            {
                ViewBag.m = v.Message;
            }
            return View(model);
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult UpdateServiceType(int? id)
        {
            try
            {
                ServiceTypeViewModel type = stb.GetbyID(Convert.ToInt32(id));
                return View(type);
            }
            catch(Exception v)
            {
                
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateServiceType(ServiceTypeViewModel model)
        {
            try
            {
                stb.UpdateServiceType(model);
                return RedirectToAction("GetAllServiceType");
            }
            catch(Exception v)
            {

            }
            return View(model);
        }
        [Authorize(Roles = "Treasure")]
        public ActionResult DeleteServiceType(int? id)
        {
            try
            {
                ServiceTypeViewModel type = stb.GetbyID(Convert.ToInt32(id));
                return View(type);
            }
            catch (Exception v)
            {

            }
            return View();
        }
        [HttpPost]
        public ActionResult DeleteServiceType(int id)
        {
            try
            {
                stb.DeleteServiceType(id);
                return RedirectToAction("GetAllServiceType");
            }
            catch(Exception v)
            {
                
            }
            return View();
        }
    }
}