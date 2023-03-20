using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class AssetRentalController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<Asset> RentalList()
        {
            List<Asset> finalList = new List<Asset>();
            List<Asset> RentaLAss = (from a in db.Assets.ToList() where a.ForRental == true select a).ToList();
            foreach (var r in RentaLAss)
            {
                AssetCharge charge = db.AssetCharges.ToList().Find(x => x.Asset_Code == r.Asset_Code);
                if (charge != null)
                {
                    finalList.Add(r);
                }
            }
            return finalList;
        }
        public ActionResult Index()
        {
            List<Asset_Rentalz> assets = db.Asset_Rentals.ToList();
            return View(assets);
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            ViewBag.feed = null;
            List<Asset_Renter> renter = (from a in db.Asset_Renters.ToList() where a.Renter_code == id select a).ToList();
            ViewBag.Renter_code = new SelectList(renter, "Renter_code", "FullName");
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            Asset_RentalViewModel asset = new Asset_RentalViewModel
            {
                Condition = "Good"
            };
            return View(asset);
        }

        [HttpPost]
        public ActionResult Create(Asset_RentalViewModel rm)
        {
            string feed = null;
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            ViewBag.Renter_code = new SelectList(db.Asset_Renters.ToList(), "Renter_code", "FullName");
            try
            {
                string HDate = rm.HireDate.ToShortDateString();
                string dateNow = DateTime.Now.ToShortDateString();

                if (Convert.ToDateTime(HDate) >= Convert.ToDateTime(dateNow))
                {

                    if (rm.quantity <= 0)
                    {
                        feed = "Invalid number of quantity..! ";
                    }
                    else
                    {
                        if (rm.Days <= 0)
                        {
                            feed = "Invalid number of Days..! ";
                        }
                        else
                        {
                            int qty = 0;
                            Asset ass = db.Assets.Find(rm.Asset_Code);
                            qty = ass.quantity - rm.quantity;
                            if (qty < 0)
                            {
                                feed = "The number of " + ass.AName + " that you want is more than the number of " + ass.AName + " that we have, we have " + ass.quantity + " " + ass.AName;
                            }
                            else
                            {
                                AssetCharge charge = db.AssetCharges.ToList().Find(x => x.Asset_Code == rm.Asset_Code);
                                DateTime returndate = rm.HireDate.AddDays(rm.Days);
                                Asset_Rentalz assets = new Asset_Rentalz
                                {
                                    Renter_code = rm.Renter_code,
                                    HireDate = rm.HireDate,
                                    ReturnDate = returndate,
                                    Amount = (charge.Rental_Amount * rm.quantity) * rm.Days,
                                    Condition = "Good",
                                    quantity = rm.quantity,
                                    Returned = false,
                                    returnedQty = 0,
                                    Asset_Code = rm.Asset_Code
                                };

                                db.Asset_Rentals.Add(assets);
                                db.SaveChanges();
                                ass.quantity = qty;
                                db.SaveChanges();

                                ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
                                ViewBag.Renter_code = new SelectList(db.Asset_Renters.ToList(), "Renter_code", "FullName");
                                return RedirectToAction("Index");
                            }

                        }
                    }


                }
                else
                {
                    feed = "You selected a passed hire date..! ";

                }

            }
            catch (Exception e)
            {
                feed += e.Message;
            }
            ViewBag.feed = feed;
            return View(rm);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset_Rentalz asse = db.Asset_Rentals.Find(id);
            if (asse == null)
            {
                return HttpNotFound();
            }

            TimeSpan span = asse.ReturnDate - asse.HireDate;
            var dayz = span.TotalDays;

            Asset_RentalViewModel ass = new Asset_RentalViewModel
            {
                Renter_code = asse.Renter_code,
                Rental_code = asse.Rental_code,
                ReturnDate = asse.ReturnDate,
                Amount = asse.Amount,
                Condition = asse.Condition,
                quantity = asse.quantity,
                Returned = asse.Returned,
                HireDate = asse.HireDate,
                Days = Convert.ToInt32(dayz),
                returnedQty = asse.returnedQty
            };
            ViewBag.QtyBefore = ass.quantity;
            ViewBag.Renter_code = new SelectList(db.Asset_Renters.ToList(), "Renter_code", "FullName");
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            return View(ass);
        }
        [HttpPost]
        public ActionResult Edit(Asset_RentalViewModel rm, FormCollection form)
        {
            string feed = null;
            ViewBag.Renter_code = new SelectList(db.Asset_Renters.ToList(), "Renter_code", "FullName");
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            try
            {
                string HDate = rm.HireDate.ToShortDateString();
                string dateNow = DateTime.Now.ToShortDateString();

                if (Convert.ToDateTime(HDate) >= Convert.ToDateTime(dateNow))
                {

                    if (rm.quantity <= 0)
                    {
                        feed = "Invalid number of quantity..!";
                    }
                    else
                    {
                        if (rm.Days <= 0)
                        {
                            feed = "Invalid number of Days..!";
                        }
                        else
                        {
                            int qtybefore = Convert.ToInt32(form["txtQtybefore"]);
                            Asset az = db.Assets.Find(rm.Asset_Code);
                            az.quantity += qtybefore;
                            db.SaveChanges();

                            int qty = 0;

                            qty = az.quantity - rm.quantity;
                            if (qty < 0)
                            {
                                //feed = "The number of " + az.AssetName + " that you want is more than the number of " + az.AssetName + " that we have, we have " + az.quantity + " " + az.AssetName;
                            }
                            else
                            {
                                AssetCharge charge = db.AssetCharges.ToList().Find(x => x.Asset_Code == rm.Asset_Code);
                                DateTime returndate = rm.HireDate.AddDays(rm.Days);
                                Asset_Rentalz ass = db.Asset_Rentals.ToList().Find(x => x.Rental_code == rm.Rental_code);

                                ass.Renter_code = rm.Renter_code;
                                ass.HireDate = Convert.ToDateTime(HDate);
                                ass.ReturnDate = returndate;
                                ass.Amount = (charge.Rental_Amount * rm.quantity) * rm.Days;
                                ass.Condition = "Good";
                                ass.quantity = rm.quantity;
                                ass.Returned = false;
                                az.quantity = qty;
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                        }
                    }


                }
                else
                {
                    feed = "You selected a passed hire date..!";

                }
            }
            catch (Exception c)
            {
                feed += c.Message;
            }
            ViewBag.feed = feed;
            ViewBag.Asset_code = new SelectList(RentalList(), "Asset_Code", "AName");
            ViewBag.Renter_code = new SelectList(db.Asset_Renters.ToList(), "Renter_code", "FullName");
            return View(rm);
        }

        public ActionResult Returned(int? id)
        {
            int remqty = 0;
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset_Rentalz asse = db.Asset_Rentals.Find(id);
            if (asse == null)
            {
                return HttpNotFound();
            }
            Asset_RentalViewModel ass = new Asset_RentalViewModel
            {
                Rental_code = asse.Rental_code,
                Asset_Code = asse.Asset_Code,
                Returned = asse.Returned
            };
            //  ViewBag.hqty=asse.quantity;
            //ViewBag.rqty = asse.returnedQty;
            remqty = asse.quantity - asse.returnedQty;
            ViewBag.remQty = remqty;
            ViewBag.Renter_code = new SelectList(db.Asset_Renters.ToList(), "Renter_code", "FullName");
            return View(ass);
        }
        [HttpPost]
        public ActionResult Returned(Asset_RentalViewModel rm, FormCollection form)
        {
            string feed = null;
            int remainQTY = Convert.ToInt32(form["txtremqyt"]);
            try
            {
                if (rm.quantity < 1)
                {
                    feed = "Invalid quantity, please enter correct quantity..!";
                }
                else
                {
                    Asset set = db.Assets.ToList().Find(x => x.Asset_Code == rm.Asset_Code);
                    if (rm.quantity > remainQTY)
                    {
                        feed = "Incorrect quantity, the are " + remainQTY + " number of " + set.AName + " need to be returned";
                    }
                    else
                    {
                        Asset_Rentalz ass = db.Asset_Rentals.ToList().Find(x => x.Rental_code == rm.Rental_code);
                        if (rm.quantity < remainQTY)
                        {
                            ass.returnedQty += rm.quantity;
                            ass.Returned = false;
                            set.quantity += rm.quantity;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ass.returnedQty += rm.quantity;
                            ass.Returned = true;
                            set.quantity += rm.quantity;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }
                }

            }
            catch (Exception v)
            {

            }
            ViewBag.feed = feed;
            ViewBag.remQty = Convert.ToInt32(form["txtremqyt"]);
            return View(rm);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset_Rentalz asset_Rentalz = db.Asset_Rentals.Find(id);
            if (asset_Rentalz == null)
            {
                return HttpNotFound();
            }
            return View(asset_Rentalz);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Asset_Rentalz asset_Rentalz = db.Asset_Rentals.Find(id);
                Asset ass = db.Assets.Find(asset_Rentalz.Asset_Code);
                db.Asset_Rentals.Remove(asset_Rentalz);
                ass.quantity += asset_Rentalz.quantity;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception c)
            {

            }
            return View();

        }
    }
}