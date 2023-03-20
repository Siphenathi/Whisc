using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSIC.Data;
using WHSIC.Model;
using WHSIC.Service;
using System.Web.Mvc;

namespace WHSIC.BusinessLogic
{
    public class DonationBusiness
    {
        DonorTypeBusiness dt = new DonorTypeBusiness();
        DonorBusiness db = new DonorBusiness();
        public List<DonationViewModel> GetAllDonation()
        {
            using (var pr = new DonationRepository())
            {
                return pr.getAll().Select(x => new DonationViewModel()
                {
                    DonationID=x.DonationID,
                    TDescription=x.DonationTypes.Description,
                    DonorDesc=x.Donor.DonorType.Description,
                    DonationtypeID =x.DonationtypeID,
                    Donor_Name = x.Donor.Name,
                    Donor_ID=x.Donor_ID,
                    Payment_Method=x.Payment_Method,
                    Date=Convert.ToDateTime(x.Date),
                    Amount=x.Amount,
                }).ToList();

            }
        }

        public void AddDonation(DonationViewModel model)
        {
            using (var pr = new DonationRepository())
            {
                var z = new Donation
                {
                    Date = model.Date.ToString(),
                    Amount =model.Amount,
                    Donor_ID=model.Donor_ID,
                    DonationtypeID=model.DonationtypeID,
                    Payment_Method=model.Payment_Method
                };
                pr.Save(z);
            }
        }
        public void DeleteDonation(int id)
        {
            using (var pr = new DonationRepository())
            {
                Donation p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }
        public DonationViewModel GetbyID(int id)
        {
            using (var pr = new DonationRepository())
            {
                Donation p = pr.GetByID(id);
                var view = new DonationViewModel();

                if (p != null)
                {
                    view.DonationID = p.DonationtypeID;
                    view.TDescription = p.DonationTypes.Description;
                    view.DonationtypeID = p.DonationtypeID;
                    view.Donor_Name = p.Donor.Name;
                    view.Donor_ID = p.Donor_ID;
                    view.Date = Convert.ToDateTime(p.Date);
                    view.Amount = p.Amount;
                }
                return view;
            }
        }
        //public void UpdateDonation(DonationViewModel model)
        //{
        //    using (var pr = new DonationRepository())
        //    {
        //        Donation p = pr.GetByID(model.DonationID);

        //        if (p != null)
        //        {   
        //            pr.Update(p);
        //        }

        //    }
            
        //}

    }
}
