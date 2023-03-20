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
    public class DonorTypeBusiness
    {
        public List<DonorTypeViewModel> GetAllDonarType()
        {
            using (var pr = new DonorTypeRepository())
            {
                return pr.getAll().Select(x => new DonorTypeViewModel()
                {
                    DonortypeID = x.DonortypeID,
                    Description = x.Description

                }).ToList();

            }
        }

        public void AddDonarType(DonorTypeViewModel model)
        {
            using (var pr = new DonorTypeRepository())
            {
                var p = new DonorType
                {
                    Description = model.Description
                };
                pr.Save(p);
            }
        }

        public void DeleteDonarType(int id)
        {
            using (var pr = new DonorTypeRepository())
            {
                DonorType p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public DonorTypeViewModel GetbyID(int id)
        {
            using (var pr = new DonorTypeRepository())
            {
                DonorType p = pr.GetByID(id);
                var view = new DonorTypeViewModel();

                if (p != null)
                {
                    view.Description = p.Description;
                }
                return view;
            }
        }
        public void UpdateDonarType(DonorTypeViewModel model)
        {
            using (var pr = new DonorTypeRepository())
            {
                DonorType p = pr.GetByID(model.DonortypeID);

                if (p != null)
                {
                    p.Description = model.Description;
                    pr.Update(p);
                }

            }
        }
    }
}
