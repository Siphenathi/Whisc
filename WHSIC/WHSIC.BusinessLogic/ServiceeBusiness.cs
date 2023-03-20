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
    public class ServiceeBusiness
    {
        public List<ServiceeViewModel> GetAllService()
        {
            using (var pr = new ServiceeRepository())
            {
                return pr.getAll().Select(x => new ServiceeViewModel()
                {
                    Service_id = x.Service_id,
                    Service_name = x.Service_name,
                    Date = x.Date,
                    Status = x.Status,
                    Quantity = x.Quantity,
                    email=x.Donor_name,
                    TypeID=x.TypeID,
                    TypeDesc=x.ServiceTypes.Description
                }).ToList();

            }
        }

        public void AddService(ServiceeViewModel model)
        {
            using (var pr = new ServiceeRepository())
            {
                var p = new Servicee
                {
                    Service_name = model.Service_name,
                    Date = model.Date,
                    Status = model.Status,
                    Quantity = model.Quantity,
                    Donor_name=model.email,
                    TypeID=model.TypeID
                };
                pr.Save(p);
            }
        }


        public ServiceeViewModel GetbyID(int id)
        {
            using (var pr = new ServiceeRepository())
            {
                Servicee p = pr.GetByID(id);
                var view = new ServiceeViewModel();

                if (p != null)
                {
                    view.Service_id = p.Service_id;
                    view.Date = p.Date;
                    view.Quantity = p.Quantity;
                    view.Service_name = p.Service_name;
                    view.Status = p.Status;
                    view.email = p.Donor_name;
                    view.TypeID = p.TypeID;
                    view.TypeDesc = p.ServiceTypes.Description;

                }
                return view;
            }
        }
        public void UpdateService(ServiceeViewModel model)
        {
            using (var pr = new ServiceeRepository())
            {
                Servicee p = pr.GetByID(model.Service_id);

                if (p != null)
                {
                    p.Service_id = model.Service_id;
                    p.Service_name = model.Service_name;
                    p.Quantity = model.Quantity;
                    p.Status = model.Status;
                    p.Date = model.Date;
                    p.Donor_name = model.email;
                    p.TypeID = model.TypeID;

                    pr.Update(p);
                }

            }
        }
    }
}
