using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using SendGrid;
using System.Net.Mail;
using WHSIC.Data;

namespace WHSIC.Controllers
{
    public class EventGuestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Postpone(int? gID, int? eID)
        {
            Guest findg = db.Guests.ToList().Find(x => x.GuestID == gID);
            Event finde = db.Events.Find(eID);
            ViewBag.gName = findg.FullName;
            ViewBag.eName = finde.EventName;
            ViewBag.gID = findg.GuestID;
            ViewBag.eID = finde.EventID;



            EventGuest model = new EventGuest
            {
                EventID = finde.EventID,
                GuestID = findg.GuestID
            };

            ViewBag.feedback = null;
            return View(model);
        }
        [HttpPost]
        public ActionResult Postpone(EventGuest model, FormCollection form)
        {
            string feed = null;
            string g = form["txtguest"];
            string e = form["txtevent"];
            int gID = Convert.ToInt16(form["txtguestID"]);
            int eID = Convert.ToInt16(form["txteventID"]);

            var datenow = DateTime.Now;
            try
            {

                EventGuest eg = db.EventGuests.ToList().Find(x => x.EventID == eID && x.GuestID == gID);
                if (eg != null)
                {
                    if (eg.Postponed == false)
                    {
                        Event ev = db.Events.Find(eID);
                        if (ev.StartDate < datenow)
                        {
                            feed = "Sorry the event has past, you cannot reply to the past event";
                        }
                        else
                        {
                            eg.Postponed = true;
                            db.SaveChanges();
                            feed = "Thanks for confirming, we will be looking forward to see you on the event. Note that the reminder will be sent to you via email two days before the event day stay blessed";
                        }

                    }
                    else if (eg.Postponed == true)
                    {
                        feed = "You already confirmed, you can only confirm once for each event!";
                    }

                }
                else
                {

                    EventGuest evg = new EventGuest
                    {
                        GuestID = gID,
                        EventID = eID,
                        Replydate = datenow,
                        Postponed = true

                    };
                    db.EventGuests.Add(evg);
                    db.SaveChanges();
                    feed = "Thanks for confirming, we will be looking forward to see you on the event. Note that the reminder will be sent to you via email two days before the event day stay blessed";


                }

            }
            catch (Exception v)
            {
                feed += v.Message;
            }

            ViewBag.feedback = feed;
            ViewBag.gName = g;
            ViewBag.eName = e;
            ViewBag.eID = eID;
            ViewBag.gID = gID;
            return View(model);
        }


        public ActionResult Create(int? gID, int? eID)
        {
            Guest findg = db.Guests.ToList().Find(x => x.GuestID == gID);
            Event finde = db.Events.Find(eID);
            ViewBag.gName = findg.FullName;
            ViewBag.eName = finde.EventName;
            ViewBag.gID = findg.GuestID;
            ViewBag.eID = finde.EventID;



            EventGuest model = new EventGuest
            {
                EventID = finde.EventID,
                GuestID = findg.GuestID
            };

            ViewBag.feedback = null;
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(EventGuest model, FormCollection form)
        {
            string feed = null;
            string g = form["txtguest"];
            string e = form["txtevent"];
            int gID = Convert.ToInt16(form["txtguestID"]);
            int eID = Convert.ToInt16(form["txteventID"]);

            var datenow = DateTime.Now;
            var formatDate = datenow.ToString("yyyy-MM-dd");

            try
            {

                EventGuest eg = db.EventGuests.ToList().Find(x => x.EventID == eID && x.GuestID == gID);
                if (eg == null)
                {
                    Event ev = db.Events.Find(eID);
                    if (ev.StartDate < DateTime.Now)
                    {
                        feed = "Sorry the event has past, you cannot reply to the past event";
                    }
                    else
                    {
                        EventGuest evg = new EventGuest
                        {
                            GuestID = gID,
                            EventID = eID,
                            Replydate = datenow
                        };
                        db.EventGuests.Add(evg);
                        db.SaveChanges();
                        feed = "Thanks for confirming, we will be looking forward to see you on the event. Note that the reminder will be sent to you via email two days before the event stay blessed!";
                    }
                }
                else
                {
                    feed = "You already confirmed, you can only confirm once for each event!";
                }


            }
            catch (Exception v)
            {
                feed += v.Message;
            }

            ViewBag.feedback = feed;
            ViewBag.gName = g;
            ViewBag.eName = e;
            ViewBag.eID = eID;
            ViewBag.gID = gID;
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task RemindeAllGuests()
        {
            List<EventGuest> AllEventsGuest = new List<EventGuest>();
            foreach (var e in db.Events.ToList())
            {
                double days = 0;
                Event finde = db.Events.Find(e.EventID);
                string sdate = finde.StartDate.ToShortDateString();
                string datenow = DateTime.Now.ToShortDateString();

                TimeSpan t = (Convert.ToDateTime(sdate) - Convert.ToDateTime(datenow));
                days = t.TotalDays;


                if (Convert.ToInt32(days) == 2)
                {
                    if (finde.postponed == true)
                    {
                        AllEventsGuest = db.EventGuests.ToList().FindAll(x => x.EventID == finde.EventID && x.Postponed == true);

                        foreach (var g in AllEventsGuest)
                        {
                            EventGuest eguest = db.EventGuests.Where(x => x.GuestID == g.GuestID && x.EventID==e.EventID).FirstOrDefault();

                            if (eguest.Reminder == false)
                            {
                                Guest gu = db.Guests.Find(g.GuestID);
                                await SendEmailReminder(e);

                                eguest.Reminder = true;
                                db.SaveChanges();
                            }

                        }
                    }
                    else
                    {
                        AllEventsGuest = db.EventGuests.ToList().FindAll(x => x.EventID == finde.EventID && x.Postponed == false);

                        foreach (var g in AllEventsGuest)
                        {
                            EventGuest eguest = db.EventGuests.Where(x => x.GuestID == g.GuestID && x.EventID == e.EventID).FirstOrDefault();

                            if (eguest.Reminder == false)
                            {
                                Guest gu = db.Guests.Find(g.GuestID);
                                await SendEmailReminder(e);

                                eguest.Reminder = true;
                                db.SaveChanges();
                            }
                        }
                    }
                }

            }

        }


        public async Task SendEmailReminder(Event ev)
        {
            foreach (var g in db.Guests.ToList())
            {
                SendGridMessage myMessage = new SendGridMessage();
                myMessage.From = new MailAddress("debuggers@outlook.com", "WHSIC");

                myMessage.AddTo(g.Guestemail);

                string subject = "Event Reminder";

                string html = "Hi " + g.FullName.Substring(0, g.FullName.IndexOf(" ")) + ", please be reminded that only 2 days left for " + ev.EventName + " event to begin, same time same place "

                     + "<td><p>We'll be looking forward to see you soon</p>";



                myMessage.Subject = subject;
                myMessage.Html = html;

                var transportWeb = new Web("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");

                //  Send the email.
                await transportWeb.DeliverAsync(myMessage);
            }
        }
    }
}