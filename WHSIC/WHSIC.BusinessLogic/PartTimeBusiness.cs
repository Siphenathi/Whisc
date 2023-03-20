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
    public class PartTimeBusiness
    {
            PositionBusiness pb = new PositionBusiness();
            public string Description(int PositionID)
            {
                PositionViewModel pv = pb.GetbyID(PositionID);

                return pv.Description;
            }
            public List<PartTimeViewModel> GetAllPartTime()
            {
                using (var pr = new PartTimeRepository())
                {
                    return pr.getAll().Select(x => new PartTimeViewModel()
                    {
                        ID = x.Basic,
                        BasicRate = x.BasicRate,
                        Description = Description(x.PositionID),
                        PositionID = x.PositionID
                    }).ToList();

                }
            }

            public void AddPartTime(PartTimeViewModel model)
            {
                using (var pr = new PartTimeRepository())
                {
                    var p = new PartTime
                    {
                        BasicRate = model.BasicRate,
                        PositionID = model.PositionID,
                    };
                    pr.Save(p);
                }
            }

            public void DeletePartTime(int id)
            {
                using (var pr = new PartTimeRepository())
                {
                    PartTime p = pr.GetByID(id);

                    if (p != null)
                    {
                        pr.Delete(p);
                    }
                }

            }

            public PartTimeViewModel GetbyID(int id)
            {
                using (var pr = new PartTimeRepository())
                {
                    PartTime p = pr.GetByID(id);
                    var view = new PartTimeViewModel();

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
            public void UpdatePartTime(PartTimeViewModel model)
            {
                using (var pr = new PartTimeRepository())
                {
                    PartTime p = pr.GetByID(model.ID);

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
