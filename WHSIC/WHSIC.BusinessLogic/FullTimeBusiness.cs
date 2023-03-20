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
    public class FullTimeBusiness
    {
        PositionBusiness pb = new PositionBusiness();
        public string Description(int PositionID)
        {
            PositionViewModel pv = pb.GetbyID(PositionID);
            return pv.Description;
        }
        public List<FullTimeViewModel> GetAllFullTime()
        {
            using (var pr = new FullTimeRepository())
            {
                return pr.getAll().Select(x => new FullTimeViewModel()
                {
                    ID = x.Basic,
                    BasicRate=x.BasicRate,
                    Description= Description(x.PositionID),
                    PositionID =x.PositionID
                }).ToList();

            }
        }

        public void AddFullTime(FullTimeViewModel model)
        {
            using (var pr = new FullTimeRepository())
            {
                var p = new FullTime
                {
                    BasicRate=model.BasicRate,
                    PositionID=model.PositionID,
                };
                pr.Save(p);
            }
        }

        public void DeleteFullTime(int id)
        {
            using (var pr = new FullTimeRepository())
            {
                FullTime p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public FullTimeViewModel GetbyID(int id)
        {
            using (var pr = new FullTimeRepository())
            {
                FullTime p = pr.GetByID(id);
                var view = new FullTimeViewModel();

                if (p != null)
                {
                    view.BasicRate = p.BasicRate;
                    view.PositionID = p.PositionID;
                    view.Description = Description(p.PositionID);
                    view.ID = p.Basic;
                }
                return view;
            }


        }
        public void UpdateFullTime(FullTimeViewModel model)
        {
            using (var pr = new FullTimeRepository())
            {
                FullTime p = pr.GetByID(model.ID);

                if (p != null)
                {
                    p.BasicRate = model.BasicRate;
                    p.PositionID = model.PositionID;
                    pr.Update(p);
                }

            }

        }
    }
}
