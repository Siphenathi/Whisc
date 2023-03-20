using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using System.Threading.Tasks;
using System.Net.Mail;
using WHSIC.Data;

namespace WHSIC.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        InternetTime it = new InternetTime();

        public ActionResult EventAttendace(int ?id)
        {
            List<EventGuest> FinalList = new List<EventGuest>();
            Event eg = db.Events.ToList().Find(x => x.EventID == id);
            if (eg.postponed == true)
            {
                FinalList = db.EventGuests.ToList().FindAll(x => x.EventID == id && x.Postponed == true);
            }
            else
            {
                FinalList = db.EventGuests.ToList().FindAll(x => x.EventID == id && x.Postponed == false);
            }
            ViewBag.Attendencenum = FinalList.Count;
            return View(FinalList);
        }
        public ActionResult Index(string feed)
        {
            ViewBag.Evname = db.Events.ToList();
            ViewBag.feed = feed;
            return View(db.Events.ToList());
        }
        [HttpPost]
        public ActionResult Index()
        {
            ViewBag.Evname = db.Events.ToList();
            int id = Convert.ToInt16(Request["Drop"]);
            List<Event> even = db.Events.ToList().FindAll(x => x.EventID == id);
            ViewBag.Evname = db.Events.ToList();
            return View(even);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,EventName,Venue,Discription,StartDate,EndDate,StartTime,EndTime")] Event @event)
        {
            string feed = null;
            if (ModelState.IsValid)
            {
                string date = it.GetNistTime().ToShortDateString();
                string edate = @event.StartDate.ToShortDateString();
                if ( Convert.ToDateTime(edate) < Convert.ToDateTime(date))
                {
                    feed = "Invalid start date, you cannot select the past date as the start date of the event";
                }
                else
            {
                db.Events.Add(@event);
                db.SaveChanges();
                Event laste = db.Events.ToList().Last();

                //await SendEventEmail(laste);
                return RedirectToAction("Index");
            }
                ViewBag.feed = feed;
           }

            return View(@event);
        }
        public async Task SendEventEmail(Event ev)
        {
            foreach (var g in db.Guests.ToList())
            {
                          
                var callbackUrl = Url.Action("Create", "EventGuests", new {gID=g.GuestID,eID= ev.EventID }, protocol: Request.Url.Scheme);

                SendGridMessage myMessage = new SendGridMessage();
                myMessage.From = new MailAddress("debuggers@outlook.com", "WHSIC");

                    myMessage.AddTo(g.Guestemail);                       

                string subject = "Invitation";

                string html = "You are kindly invited to the " + ev.EventName + " which will be held at " + ev.Venue + " from the " + " " +FormatDate(ev.StartDate)+" until the "+ FormatDate(ev.EndDate) + " at" + " " + ev.StartTime + " to" + " " + ev.EndTime
                             + "<td><p>Please confirm if You are Coming, by clicking <a href=\"" + callbackUrl + "\">here</a></p>";


                myMessage.Subject = subject;
                myMessage.Html = html;

                var transportWeb = new Web("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");

                //  Send the email.
                await transportWeb.DeliverAsync(myMessage);
            }

        }
        public async Task EventCancelled(Event ev)
        {
            foreach (var g in db.Guests.ToList())
            {
                SendGridMessage myMessage = new SendGridMessage();
                myMessage.From = new MailAddress("debuggers@outlook.com", "WHSIC");

                myMessage.AddTo(g.Guestemail);

                string subject = "Cancelled Event";

                string html = "Please note that the" + " " + ev.EventName + " " + "Event that was suppose to be held at " + ev.Venue + " has been cancelled "
                     +"<p>We apologise for inconvinience</p>";



                myMessage.Subject = subject;
                myMessage.Html = html;

                var transportWeb = new Web("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");

                //  Send the email.
                await transportWeb.DeliverAsync(myMessage);
            }
        }
        public async Task Postpone(Event ev)
        {
            foreach (var g in db.Guests.ToList())
            {
               
                var callbackUrl = Url.Action("Postpone", "EventGuests", new { gID = g.GuestID, eID = ev.EventID }, protocol: Request.Url.Scheme);

                SendGridMessage myMessage = new SendGridMessage();
                myMessage.From = new MailAddress("debuggers@outlook.com", "WHSIC");



                myMessage.AddTo(g.Guestemail);

                //    List<String> recipients = new List<String>
                //{
                //     @"Siyabonga Nkosi <snkosi524@gmail.com>"
                //};
                //    myMessage.AddTo(recipients);

                string subject = "Invitation";

                string html = "Please note that the" +" "+ ev.EventName+" "+ "Event has been posponed to the"+" "+ FormatDate(ev.StartDate) + " " + "until the" +" "+ FormatDate(ev.EndDate) + " "+ "At" +" "+ ev.StartTime + " " +"to"+" "+ ev.EndTime +
                    ". " +"It will be held At"+" "+ ev.Venue+". "+"We apologise for inconvinience."
 
                     + "<td><p>Please confirm if You are Coming, by clicking <a href=\"" + callbackUrl + "\">here</a></p>";



                myMessage.Subject = subject;
                myMessage.Html = html;

                var transportWeb = new Web("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");

                //  Send the email.
                await transportWeb.DeliverAsync(myMessage);
            }
        }

        public ActionResult Edit(int? id, string button)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.btn = button;
            ViewBag.feed = null;
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Event @event, FormCollection form)
        {
            string logic = form["logic"];
            string feed = null;

            if(@event.StartDate<DateTime.Now)
            {
                feed = "Invalid start date, you cannot select the past date as the start date of the event";
            }
            else
            {
                if (logic == "Edit")
                {
                    db.Entry(@event).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Event eg = db.Events.Find(@event.EventID);
                    eg.StartDate = @event.StartDate;
                    eg.EndDate = @event.EndDate;
                    eg.StartTime = @event.StartTime;
                    eg.EndTime = @event.EndTime;
                    eg.Venue = @event.Venue;
                    eg.postponed = true;
                    db.SaveChanges();
                    await Postpone(@event);

                    List<EventGuest> alleventg = db.EventGuests.ToList().FindAll(x=>x.EventID==@event.EventID);

                    foreach(var guest in alleventg)
                    {
                        EventGuest eve = db.EventGuests.Where(x => x.GuestID == guest.GuestID && x.EventID == @event.EventID).FirstOrDefault();
                        eve.Reminder = false;
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
            }
            
            ViewBag.btn = logic;
            ViewBag.feed = feed;
            return View(@event);
        }

        public string FormatDate(DateTime date)
        {
            string findate = null;
            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            if (day == 1)
            {
                findate += day + "st of ";
            }
            else if (day == 2)
            {
                findate += day + "nd of ";
            }
            else if (day == 3)
            {
                findate += day + "rd of ";
            }
            else if (day > 3)
            {
                findate += day + "th of ";
            }

            if (month == 1)
            {
                findate += "January ";
            }
            else if (month == 2)
            {
                findate += "February ";
            }
            else if (month == 3)
            {
                findate += "March ";
            }
            else if (month == 4)
            {
                findate += "April ";
            }
            else if (month == 5)
            {
                findate += "May ";
            }
            else if (month == 6)
            {
                findate += "June ";
            }
            else if (month == 7)
            {
                findate += "july ";
            }
            else if (month == 8)
            {
                findate += "August ";
            }
            else if (month == 9)
            {
                findate += "September ";
            }
            else if (month == 10)
            {
                findate += "October ";
            }
            else if (month == 11)
            {
                findate += "November ";
            }
            else if (month == 12)
            {
                findate += "December ";
            }
            return findate + year;
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            await EventCancelled(@event);
            return RedirectToAction("Index");
        }
        public ActionResult Confirm()
        {
            return View();
        }
        public async Task<ActionResult> Invite(int id)
        {
            string feed = null;
            Event e = db.Events.Find(id);

            if (e.Invite == true)
            {
                feed = "Sorry Invitations has been sent Already!!!";
            }
            else
            {
                e.Invite = true;
                db.SaveChanges();

                await SendEventEmail(e);
                feed = "Invitations has been sent to all guest";
            }
            return RedirectToAction("Index", new { feed });
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
