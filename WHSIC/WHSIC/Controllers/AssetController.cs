
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using WHSIC.Data;
using WHSIC.Model;

namespace Asset_Rental.Controllers
{
    public class AssetController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(db.Assets.ToList());
        }
        public ActionResult RentalAssets()
        {
            List<Asset> RentaLAss = (from a in db.Assets.ToList() where a.ForRental == true select a).ToList();
            return View(RentaLAss);
        }
        public ActionResult NonRentalAssets()
        {
            List<Asset> RentaLAss = (from a in db.Assets.ToList() where a.ForRental == false select a).ToList();
            return View(RentaLAss);

        }
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Asset_Categories.ToList(), "CategoryID", "CName" );
            ViewBag.supply_code = new SelectList(db.Suppliers.ToList(), "supply_code", "name");
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        public ActionResult Create(AssetViewModel model)
        {
            ViewBag.CategoryID = new SelectList(db.Asset_Categories.ToList(), "CategoryID", "CName");
            ViewBag.supply_code = new SelectList(db.Suppliers.ToList(), "supply_code", "name");
            string feed = null;
            try
            {
                if (ModelState.IsValid)
                {
                    string pDate = model.PurchaseDate.ToShortDateString();
                    string dateNow = DateTime.Now.ToShortDateString();

                    if (Convert.ToDateTime(pDate) <= Convert.ToDateTime(dateNow))
                    {

                        if (model.quantity <= 0)
                        {
                            feed = "Invalid number of quantity..! ";
                        }
                        else
                        {
                            if (model.Warranty <= 0 || model.LifeSpan <= 0)
                            {
                                feed = "Invalid warranty or lifespan, please enter correct value..!";
                            }
                            else
                            {

                                Asset ass = new Asset
                                {
                                    CategoryID = model.CategoryID,
                                    PurchaseDate = model.PurchaseDate,
                                    LifeSpan = model.LifeSpan + " " + model.LifeSpanFormat,
                                    Warranty = model.Warranty + " Months",
                                    UnitPrice =model.Amount,
                                    Amount = model.Amount * model.quantity,
                                    ForRental = model.ForRental,
                                    quantity = model.quantity,
                                    supply_code=model.supply_code,
                                    AName=model.AName
                                };
                                db.Assets.Add(ass);
                                db.SaveChanges();
                                int d = 0;
                                 int x=d;

                                return RedirectToAction("Index");
                            }
                        }


                    }
                    else
                    {
                        feed = "Invalid purchase date, the date selected for purchase date has not yet arrived..! ";

                    }
                }
                
            }
            catch(Exception b)
            {
                feed += b.Message;
            }
            ViewBag.feed = feed;
            ViewBag.CategoryID = new SelectList(db.Asset_Categories.ToList(), "CategoryID", "CName");
            ViewBag.supply_code = new SelectList(db.Suppliers.ToList(), "supply_code", "name");
            return View(model);
        }

        public ActionResult Edit(int?id)
        {
            ViewBag.feed = null;
            AssetViewModel assModel = new AssetViewModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset ass = db.Assets.Find(id);
            if (ass == null)
            {
                return HttpNotFound();
            }
            else
            {
                assModel.CategoryID = ass.CategoryID;
                assModel.PurchaseDate = ass.PurchaseDate;
                assModel.LifeSpan = Convert.ToInt32(ass.LifeSpan.Substring(0, ass.LifeSpan.IndexOf(" ")));
                assModel.Warranty = Convert.ToInt32(ass.Warranty.Substring(0, ass.Warranty.IndexOf(" ")));
                assModel.LifeSpanFormat = ass.LifeSpan.Substring(ass.LifeSpan.IndexOf(" ")+1);
                assModel.Amount = (ass.Amount/ass.quantity);
                assModel.ForRental = ass.ForRental;
                assModel.quantity = ass.quantity;
                assModel.Asset_Code = ass.Asset_Code;
                assModel.AName = ass.AName;
                assModel.supply_code = ass.supply_code;
                assModel.UnitPrice = ass.UnitPrice;
            }
            ViewBag.CategoryID = new SelectList(db.Asset_Categories.ToList(), "CategoryID", "CName");
            ViewBag.supply_code = new SelectList(db.Suppliers.ToList(), "supply_code", "name");

            return View(assModel);
        }
        [HttpPost]
        public ActionResult Edit(AssetViewModel model)
        {
            string feed = null;
            try
            {
                ViewBag.CategoryID = new SelectList(db.Asset_Categories.ToList(), "CategoryID", "CName");
                ViewBag.supply_code = new SelectList(db.Suppliers.ToList(), "supply_code", "name");
                string pDate = model.PurchaseDate.ToShortDateString();
                string dateNow = DateTime.Now.ToShortDateString();

                if (Convert.ToDateTime(pDate) <= Convert.ToDateTime(dateNow))
                {

                    if (model.quantity <= 0)
                    {
                        feed = "Invalid number of quantity..! ";
                    }
                    else
                    {
                        if (model.Warranty <= 0 || model.LifeSpan <= 0)
                        {
                            feed = "Invalid warranty or lifespan, please enter correct value..! ";
                        }
                        else
                        {
                            Asset ass = db.Assets.Find(model.Asset_Code);

                            ass.CategoryID = model.CategoryID;
                            ass.PurchaseDate = model.PurchaseDate;
                            ass.LifeSpan = model.LifeSpan + " " + model.LifeSpanFormat;
                            ass.Warranty = model.Warranty + " Months";
                            ass.Amount = model.Amount * model.quantity;
                            ass.ForRental = model.ForRental;
                            ass.quantity = model.quantity;
                            ass.AName = model.AName;
                            ass.UnitPrice = model.UnitPrice;
                            ass.supply_code = model.supply_code;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }


                }
                else
                {
                    feed = "Invalid purchase date, the date selected for purchase date has not yet arrived..! ";

                }
            }
            catch (Exception b)
            {
                feed += b.Message;
            }
            ViewBag.feed = feed;
            ViewBag.CategoryID = new SelectList(db.Asset_Categories.ToList(), "CategoryID", "CName");
            ViewBag.supply_code = new SelectList(db.Suppliers.ToList(), "supply_code", "name");
            return View(model);
        }

        public ActionResult Delete(int?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset_Rentalz = db.Assets.Find(id);
            if (asset_Rentalz == null)
            {
                return HttpNotFound();
            }
            return View(asset_Rentalz);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset asset_Rentalz = db.Assets.Find(id);
            db.Assets.Remove(asset_Rentalz);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AssetCharge(int?id)
        {
            AssetChargeViewModel charge = new AssetChargeViewModel();
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset_Rentalz = db.Assets.Find(id);
            if (asset_Rentalz == null)
            {
                return HttpNotFound();
            }
            else
            {
                
                charge.Asset_Code = Convert.ToInt32(id);
            }
            ViewBag.AssetName = asset_Rentalz.AName;
            ViewBag.Cost = asset_Rentalz.UnitPrice;
            return View(charge);
        }
        [HttpPost]
        public ActionResult AssetCharge(AssetChargeViewModel model)
        {
            string feed = null;
            try
            {
                if(model.PenaltyRate>100 || model.PenaltyRate<=0)
                {
                    feed = "Invalid penalty rate, the penalty rate must not be greater than 100% and also not less or equal to 0";
                }
                else
                {
                    if(model.Rental_Amount<=0)
                    {
                        feed = "Invalid amount, the amount must not be less or equal to 0";
                    }
                    else
                    {
                        AssetCharge ch = db.AssetCharges.ToList().Find(x => x.Asset_Code == model.Asset_Code);
                        if(ch!=null)
                        {
                            feed = "The asset charge for this asset was added, only one asset charge for each asset ";
                        }
                        else
                        {
                            AssetCharge charge = new AssetCharge();
                            charge.Asset_Code = model.Asset_Code;
                            charge.Rental_Amount = model.Rental_Amount;
                            charge.PenaltyRate = model.PenaltyRate;
                            db.AssetCharges.Add(charge);
                            db.SaveChanges();
                            return RedirectToAction("RentalAssets");
                        }

                        
                    }
                }
            }
            catch(Exception v)
            {

            }
            ViewBag.feed = feed;
            return View();
        }
        public ActionResult UpdateAssetCharge(int?id)
        {
            AssetChargeViewModel charge = new AssetChargeViewModel();
            ViewBag.feed = null;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetCharge ch = db.AssetCharges.ToList().Find(x => x.Asset_Code == Convert.ToInt32(id));
            if (ch == null)
            {
                return HttpNotFound();
            }
            else
            {
                charge.Asset_Code = Convert.ToInt32(id);
                charge.charge_ID = ch.charge_ID;
                charge.PenaltyRate = ch.PenaltyRate;
                charge.Rental_Amount = ch.Rental_Amount;

            }
            return View(charge);
        }
        [HttpPost]
        public ActionResult UpdateAssetCharge(AssetChargeViewModel model)
        {
            string feed = null;
            try
            {
                if (model.PenaltyRate > 100 || model.PenaltyRate <= 0)
                {
                    feed = "Invalid penalty rate, the penalty rate must not be greater than 100% and also not less or equal to 0";
                }
                else
                {
                    if (model.Rental_Amount <= 0)
                    {
                        feed = "Invalid amount, the amount must not be less or equal to 0";
                    }
                    else
                    {
                            AssetCharge charge = db.AssetCharges.Find(model.charge_ID);
                            charge.Asset_Code = model.Asset_Code;
                            charge.Rental_Amount = model.Rental_Amount;
                            charge.PenaltyRate = model.PenaltyRate;
                            db.SaveChanges();
                            return RedirectToAction("RentalAssets");
                    }
                }
            }
            catch (Exception v)
            {

            }
            ViewBag.feed = feed;
            return View();
        }
        public ActionResult AllCharges()
        {
            var ass =new List<AssetChargeViewModel>();
            foreach(var c in db.AssetCharges.ToList())
            {
                AssetChargeViewModel asset = new AssetChargeViewModel
                {
                    charge_ID=c.charge_ID,
                    Asset_Code=c.Asset_Code,
                    PenaltyRate=c.PenaltyRate,
                    Rental_Amount=c.Rental_Amount
                };

                ass.Add(asset);
            }
            return View(ass);
        }
        public ActionResult ChargeDetails(int?id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetCharge asset_Rentalz = db.AssetCharges.ToList().Find(x=>x.Asset_Code==id);
            if (asset_Rentalz == null)
            {
                return HttpNotFound();
            }
            return View(asset_Rentalz);
        }

    }
}