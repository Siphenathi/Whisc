
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.Data;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class SupplierController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult SupplierList()
        {
            return View(db.Suppliers.ToList());
        }

        public ActionResult Delete(int ?id)
        {
            Supplier sup = db.Suppliers.ToList().Find(x => x.supply_code == id);

            SupplierViewModel su = new SupplierViewModel
            {
                name = sup.name,
                contact = sup.contact,
                address = sup.address,
                email = sup.email
            };

            return View(su);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string feed = null;
            try
            {
                Supplier sup = db.Suppliers.Find(id);
                db.Suppliers.Remove(sup);
                db.SaveChanges();
                return RedirectToAction("SupplierList");
            }
            catch(Exception c)
            {
                feed += c.Message;
            }
            ViewBag.feed = feed;
            return View();
        }
        public ActionResult Edit(int id)
        {
            Supplier sup = db.Suppliers.ToList().Find(x => x.supply_code == id);

            SupplierViewModel su = new SupplierViewModel
            {
                name=sup.name,
                contact=sup.contact,
                address=sup.address,
                email=sup.email,
                supply_code=sup.supply_code
            };

            return View(su);
        }
        [HttpPost]
        public  ActionResult Edit(SupplierViewModel model)
        {
            string feed = null;
            try
            {
                Supplier sup = db.Suppliers.ToList().Find(x=>x.supply_code==model.supply_code);
                sup.name = model.name;
                sup.contact = model.contact;
                sup.address = model.address;
                sup.email = model.email;
                sup.supply_code = sup.supply_code;
                db.SaveChanges();
                return RedirectToAction("SupplierList");
            }
            catch(Exception v)
            {
                feed += v.Message;
            }
            ViewBag.feed = feed;
            return View();
        }

        public ActionResult AddSupplier()
        {
            ViewBag.feed = null;
            return View();
        }
        [HttpPost]
        public ActionResult AddSupplier(SupplierViewModel model)
        {
            string feed = null;
            try
            {
                Supplier sup = new Supplier
                {
                    name=model.name,
                    contact=model.contact,
                    address=model.address,
                    email=model.email
                };
                db.Suppliers.Add(sup);
                db.SaveChanges();
                Supplier supl = db.Suppliers.ToList().Last();
                return RedirectToAction("SupplierList");
            }
            catch(Exception v)
            {
                feed += v.Message;
            }
            ViewBag.feed = feed;
            return View();
        }

    }
}