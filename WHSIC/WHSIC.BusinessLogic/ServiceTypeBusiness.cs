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
    public  class ServiceTypeBusiness
    {
        public List<ServiceTypeViewModel> GetAllServiceType()
        {
            using (var pr = new ServiceTypeRepository())
            {
                return pr.getAll().Select(x => new ServiceTypeViewModel()
                {
                    TypeID=x.TypeID,
                    Description=x.Description,
                    dismiss=x.dismiss
                }).ToList();

            }
        }

        public void AddServiceType(ServiceTypeViewModel model)
        {
            using (var pr = new ServiceTypeRepository())
            {
                var p = new ServiceType
                {
                    TypeID = model.TypeID,
                    Description = model.Description,
                    dismiss = model.dismiss
                };
                pr.Save(p);
            }
        }


        public ServiceTypeViewModel GetbyID(int id)
        {
            using (var pr = new ServiceTypeRepository())
            {
                ServiceType p = pr.GetByID(id);
                var view = new ServiceTypeViewModel();

                if (p != null)
                {
                    view.TypeID = p.TypeID;
                    view.Description = p.Description;
                    view.dismiss = p.dismiss;
                }
                return view;
            }
        }
        public void UpdateServiceType(ServiceTypeViewModel model)
        {
            using (var pr = new ServiceTypeRepository())
            {
                ServiceType p = pr.GetByID(model.TypeID);

                if (p != null)
                {
                    p.Description = model.Description;
                    p.dismiss = model.dismiss;

                    pr.Update(p);
                }

            }
        }

        public void DeleteServiceType(int id)
        {
            using (var pr = new ServiceTypeRepository())
            {
                ServiceType p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }
    }
}
