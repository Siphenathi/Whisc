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
    public class AlgorithmDatesBusiness
    {
        public List<AlgorithmDatesViewModel> GetAllAlgorithmDates()
        {
            using (var pr = new AlgorithmDateRepository())
            {
                return pr.getAll().Select(x => new AlgorithmDatesViewModel()
                {
                    dateID = x.Id,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();

            }
        }

        public void AddAlgorithmDates(AlgorithmDatesViewModel model)
        {
            using (var pr = new AlgorithmDateRepository())
            {
                var p = new AlgorithmDates
                {
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                pr.Save(p);
            }
        }

        public AlgorithmDatesViewModel GetbyID(int id)
        {
            using (var pr = new AlgorithmDateRepository())
            {
                AlgorithmDates p = pr.GetByID(id);
                var view = new AlgorithmDatesViewModel();

                if (p != null)
                {
                    view.StartDate = p.StartDate;
                    view.EndDate = p.EndDate;
                    view.dateID = p.Id;
                }
                return view;
            }
        }

        public void UpdateAlgorithmDates(AlgorithmDatesViewModel model)
        {
            using (var pr = new AlgorithmDateRepository())
            {
                AlgorithmDates p = pr.GetByID(model.dateID);
                if (p != null)
                {
                    p.EndDate = model.EndDate;
                    p.StartDate = model.StartDate;
                    pr.Update(p);
                }

            }
        }

    }
}
