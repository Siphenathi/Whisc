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
    public class DonationTypeBusiness
    {
        public List<DonationTypeViewModel> GetAllDonationType()
        {
            using (var pr = new DonationTypeRepository())
            {
                return pr.getAll().Select(x => new DonationTypeViewModel()
                {
                    DonationtypeID = x.DonationtypeID,
                    Description = x.Description

                }).ToList();

            }
        }

        public void AddDonationType(DonationTypeViewModel model)
        {
            using (var pr = new DonationTypeRepository())
            {
                var p = new DonationType
                {
                    Description = model.Description
                };
                pr.Save(p);
            }
        }

        public void DeleteDonationType(int id)
        {
            using (var pr = new DonationTypeRepository())
            {
                DonationType p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public DonationTypeViewModel GetbyID(int id)
        {
            using (var pr = new DonationTypeRepository())
            {
                DonationType p = pr.GetByID(id);
                var view = new DonationTypeViewModel();

                if (p != null)
                {
                    view.Description = p.Description;
                }
                return view;
            }
        }
        public void UpdateDonationTypes(DonationTypeViewModel model)
        {
            using (var pr = new DonationTypeRepository())
            {
                DonationType p = pr.GetByID(model.DonationtypeID);

                if (p != null)
                {
                    p.Description = model.Description;
                    pr.Update(p);
                }

            }

        }
    }
}
