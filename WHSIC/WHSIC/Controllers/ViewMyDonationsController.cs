using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHSIC.BusinessLogic;
using WHSIC.Model;

namespace WHSIC.Controllers
{
    public class ViewMyDonationsController : Controller
    {
        ServiceeBusiness sb = new ServiceeBusiness();
        DonorBusiness _db = new DonorBusiness();
        DonationBusiness donB = new DonationBusiness();

        public ActionResult GetAllMyDonations()
        {
            MyDonations myclas = new MyDonations();
            try
            {
                int c = 0, dn = 0;
                List<DonationViewModel> DonationList = new List<DonationViewModel>();
                List<ServiceeViewModel> ServiceList = new List<ServiceeViewModel>();
                string email = User.Identity.Name;
                DonorViewModel don = _db.GetAllDonor().Find(x => x.Email == email);

                //List<ServiceeViewModel> MyServices = sb.GetAllService().FindAll(x => x.email == email);

                ServiceList = (from e in sb.GetAllService()
                               where e.email == email
                               select e).ToList();

                DonationList = (from d in donB.GetAllDonation()
                                where d.Donor_ID == don.Donor_ID
                                select d).ToList();
                c = ServiceList.Count;
                dn = DonationList.Count;

                ViewBag.c = c;
                ViewBag.dn = dn;
                //if (DonationList.Count == 0)
                //{

                //}
                //else
                //{
                //    myclas.CashDonations = DonationList;   
                //}

                //if (ServiceList.Count==0)
                //{

                //}
                //else
                //{
                //    myclas.Service = ServiceList;
                //}

                myclas.CashDonations = DonationList;
                myclas.Service = ServiceList;

                //List<DonationViewModel> MyDonation = donB.GetAllDonation().FindAll(x => x.Donor_ID == don.Donor_ID);
                //ViewBag.ServiceList = MyServices;
                //ViewBag.ServiceCount = MyServices.Count;
                //ViewBag.CashCount = MyDonation.Count;
                //ViewBag.CashList = MyDonation;
            }
            catch(Exception v)
            {

            }
            return View(myclas);
        }
    }
}