using SendGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;

namespace WHSIC.Controllers
{
    public class JobController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        InternetTime it = new InternetTime();

        // GET: /Job/
        public ActionResult Index()
        {
            return View(db.Jobs.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: /Job/Create
        public ActionResult Create()
        {
            ViewBag.Msgg = null;
            return View();
        }
       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Job job)
        {
            string feed = null;
            if (ModelState.IsValid)
            {
                string sdate = it.GetNistTime().ToShortDateString();
                string cdate = job.ClosingDate.ToShortDateString();
                if (Convert.ToDateTime(cdate) < Convert.ToDateTime(sdate))
                {
                    feed = "Closing Date cannot be less than the Current Date";
                }
                else
                {
                    db.Jobs.Add(job);
                    db.SaveChanges();
                    await SendEventEmail(job);
                    return RedirectToAction("Index");
                }
                
            }
            ViewBag.Msgg = feed;
            return View(job);
        }
        public async Task SendEventEmail(Job jo)
        {
            foreach (var subscriber in db.Subscibers.ToList())
            {
                var callbackUrl = Url.Action("JobView","Subscribe", routeValues: null, protocol: Request.Url.Scheme);

            SendGridMessage myMessage = new SendGridMessage();
            myMessage.From = new MailAddress("debuggers@outlook.com", "WHSIC");

           
                myMessage.AddTo(subscriber.Email);

                string subject = "Vacancy";

                string html = "Dear " + subscriber.UserName + "." + " We are glad to inform you that there are 1 or more open positions at WHSIC."
                      + "<td><p>To see the vacancy details,Click <a href=\"" + callbackUrl + "\">here</a></p>";




                myMessage.Subject = subject;
                myMessage.Html = html;

                var transportWeb = new Web("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");

                //  Send the email.
                await transportWeb.DeliverAsync(myMessage);
            }
        }
        // GET: /Job/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: /Job/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="JobID,Position,Requirements,Duties,ClosingDate,Salary,PositionStatus,ApplyingInfo")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        // GET: /Job/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: /Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Job job = db.Jobs.Find(id);
            db.Jobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
