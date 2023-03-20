using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSIC.Service;
using WHSIC.Model;
using WHSIC.Data;

namespace WHSIC.BusinessLogic
{
    public class PositionBusiness
    {
        public List<PositionViewModel> GetAllPositions()
        {
            using (var pr = new PositionRepository())
            {
                return pr.getAll().Select(x => new PositionViewModel()
                {
                    Description=x.Description,
                    PositionID=x.PositionID
                    

                }).ToList();

            }
        }

        public void AddPositions(PositionViewModel model)
        {
            using (var pr = new PositionRepository())
            {
                var p = new Position
                {
                  Description=model.Description,
                 
                  
                };
                pr.Save(p);
            }
        }

        public void DeletePositions(int id)
        {
            using (var pr = new PositionRepository())
            {
                Position p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public PositionViewModel GetbyID(int id)
        {
            using (var pr = new PositionRepository())
            {
                Position p = pr.GetByID(id);
                var view = new PositionViewModel();

                if (p != null)
                {
                    view.Description = p.Description;
                    view.PositionID = p.PositionID;
                }
                return view;
            }


        }
        public void UpdatePositions(PositionViewModel model)
        {
            using (var pr = new PositionRepository())
            {
                Position p = pr.GetByID(model.PositionID);

                if (p != null)
                {
                    p.Description = model.Description;
                    pr.Update(p);
                }

            }

        }
    }
}
