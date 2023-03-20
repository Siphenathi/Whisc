using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSIC.Data;
using WHSIC.Model;
using WHSIC.Service;

namespace WHSIC.BusinessLogic
{
    public class DonorBusiness
    {
        DonorTypeBusiness dt = new DonorTypeBusiness();

        public string Description(int id)
        {
            string desc = "";
            DonorTypeViewModel d = dt.GetbyID(id);
            desc = d.Description;
            return desc;
        }
        public List<DonorViewModel> GetAllDonor()
        {
            using (var pr = new DonorRepository())
            {
                return pr.getAll().Select(x => new DonorViewModel()
                {
                    Donor_ID = x.Donor_ID,
                    DonortypeID=x.DonortypeID,
                    Description=Description(x.DonortypeID),
                    Name=x.Name,
                    Email=x.Email,
                    Contact=x.Contact,
                    Address=x.Address,
                    tax_no=x.tax_no,
                    Image=x.Image

                }).ToList();

            }
        }

        public void AddDonor(DonorViewModel model)
        {
            using (var pr = new DonorRepository())
            {
                var p = new Donor
                {
                    Donor_ID = model.Donor_ID,
                    DonortypeID = model.DonortypeID,
                    Name = model.Name,
                    Email=model.Email,
                    Contact = model.Contact,
                    Address = model.Address,
                    tax_no = model.tax_no.ToString(),
                    Image = model.Image
                };
                pr.Save(p);
            }
        }
        public void DeleteDonor(int id)
        {
            using (var pr = new DonorRepository())
            {
                Donor p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }
        public DonorViewModel GetbyID(int id)
        {
            using (var pr = new DonorRepository())
            {
                Donor p = pr.GetByID(id);
                var view = new DonorViewModel();

                if (p != null)
                {
                    view.Donor_ID = p.Donor_ID;
                    view.DonortypeID = p.DonortypeID;
                    view.Description = Description(p.DonortypeID);
                    view.Email = p.Email;
                    view.Name = p.Name;
                    view.Contact = p.Contact;
                    view.Address = p.Address;
                    view.tax_no = p.tax_no;
                    view.Image = p.Image;
                }
                return view;
            }
        }
        public void UpdateDonor(DonorViewModel model)
        {
            using (var pr = new DonorRepository())
            {
                Donor p = pr.GetByID(model.DonortypeID);

                if (p != null)
                {

                    p.Name = model.Name;
                    p.Email = model.Email;
                    p.Address = model.Address;
                    p.Contact = model.Contact;
                    p.Image = model.Image;
                    p.tax_no = model.tax_no.ToString();

                    pr.Update(p);

                }

            }
        }
    }
}
