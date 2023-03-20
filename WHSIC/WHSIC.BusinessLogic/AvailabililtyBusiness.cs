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
    public class AvailabililtyBusiness
    {

        public List<AvailabilityViewModel> GetAllAvailability()
        {
            using (var pr = new AvailabilityRepository())
            {
                return pr.getAll().Select(x => new AvailabilityViewModel()
                {
                   Id = x.Id,
                   pastorID = x.pastorID,
                   Startdate = x.Startdate,
                   EndDate = x.EndDate,
                   PastorName = x.Pastors.PastorFullName
                }).ToList();

            }
        }

        public void AddAvailability(AvailabilityViewModel model)
        {
            using (var pr = new AvailabilityRepository())
            {
                var p = new Availability
                {
                    Startdate = model.Startdate,
                    EndDate = model.EndDate,
                    pastorID = model.pastorID
                };
                pr.Save(p);
            }
        }

        public void DeleteAvailability(int id)
        {
            using (var pr = new AvailabilityRepository())
            {
                Availability p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public AvailabilityViewModel GetbyID(int id)
        {
            using (var pr = new AvailabilityRepository())
            {
                Availability p = pr.GetByID(id);
                var view = new AvailabilityViewModel();

                if (p != null)
                {
                    view.pastorID = p.pastorID;
                    view.Startdate = p.Startdate;
                    view.EndDate = p.EndDate;
                    view.PastorName = p.Pastors.PastorFullName;
                    view.Id = p.Id;
                }
                return view;
            }
        }

        public void UpdateAvailability(AvailabilityViewModel model)
        {
            using (var pr = new AvailabilityRepository())
            {
                Availability p = pr.GetByID(model.Id);
                if (p != null)
                {
                    p.pastorID = model.pastorID;
                    p.Startdate = model.Startdate;
                    p.EndDate = model.EndDate;
                    pr.Update(p);
                }

            }
        }
    }
}
