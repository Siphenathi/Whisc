using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Timers;
using Microsoft.Ajax.Utilities;
using WHSIC.BusinessLogic;
using WHSIC.Controllers;
using WHSIC.Model;

namespace WHSIC
{
    public class MvcApplication : HttpApplication
    {
        private SermonAlgorithm sa = new SermonAlgorithm();
        private Recurring_Expense re = new Recurring_Expense();
        EventGuestsController eguest = new EventGuestsController();
        CalculateCharge cc = new CalculateCharge();
        RoleController rc = new RoleController();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Timer timer = new Timer();
            //20 seconds
            timer.Interval = 20000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }


        public void Init(HttpApplication context)
        {
            context.BeginRequest += (Application_BeginRequest);
        }

        private void Application_BeginRequest(object source, EventArgs e)
        {
            AdminController _ac = new AdminController();
            var context = ((HttpApplication)source).Context;
            var ipAddress = context.Request.UserHostAddress;
            _ac.Addvisitor(ipAddress,GetNistTime());
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (sa.LastMonday().Equals(GetNistTime().ToShortDateString()))
            {
                sa.CreateSermon(rc.AdminContact());
            }
            re.RecurringExpensePayment();
            await eguest.RemindeAllGuests();
            //Add charge
            cc.DoTheWork();
        }


        public static DateTime GetNistTime()
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://nist.time.gov/actualtime.cgi?lzbc=siqm9b");
                request.Method = "GET";
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader stream = new StreamReader(response.GetResponseStream());
                    string html = stream.ReadToEnd();
                    string time = Regex.Match(html, @"(?<=\btime="")[^""]*").Value;
                    double milliseconds = Convert.ToInt64(time) / 1000.0;
                    dateTime = new DateTime(1970, 1, 1).AddMilliseconds(milliseconds).ToLocalTime();
                }
            }
            catch (Exception c)
            {

            }
            return dateTime;
        }
    }
}
