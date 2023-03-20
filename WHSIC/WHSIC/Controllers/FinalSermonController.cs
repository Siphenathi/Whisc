using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;

namespace WHSIC.Controllers
{
    public class FinalSermonController : Controller
    {
        private FinalSermonBusiness FSB = new FinalSermonBusiness();
        public ActionResult AdvertiseSermon()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Terrace;

            scheduler.Config.first_hour = 06;
            scheduler.Config.last_hour = 17;


            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }

        public ContentResult Data()
        {
            var apps = FSB.GetAllFinalSermons();
            return new SchedulerAjaxData(apps);
        }

    }
}