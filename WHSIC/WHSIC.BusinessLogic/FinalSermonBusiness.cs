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
    public class FinalSermonBusiness
    {
        public List<FinSermonViewModel> GetAllFinalSermons()
        {
            using (var pr = new FinalSermonRepository())
            {
                return pr.getAll().Select(x => new FinSermonViewModel()
                {
                    ID = x.sermID,
                    Date = Convert.ToDateTime(x.Date),
                    content = x.content,
                    StartTime = Convert.ToDateTime(x.StartTime),
                    EndTime = Convert.ToDateTime(x.EndTime),
                    pastorName = x.PastorName,
                    branchID = x.BranchID,
                    Description = x.Description,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate

                }).ToList();

            }
        }

        public void AddFinalSermon(FinSermonViewModel model)
        {
            using (var pr = new FinalSermonRepository())
            {
                var p = new FinalSermon
                {
                    Date = model.Date.ToShortDateString(),
                    content = model.content,
                    StartTime = model.StartTime.ToShortTimeString(),
                    EndTime = model.EndDate.ToShortTimeString(),
                    PastorName = model.pastorName,
                    BranchID = model.branchID,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate
                };
                pr.Save(p);
            }
        }
        public FinSermonViewModel GetbyID(int id)
        {
            using (var pr = new FinalSermonRepository())
            {
                FinalSermon p = pr.GetByID(id);
                var view = new FinSermonViewModel();

                if (p != null)
                {
                    view.ID = p.sermID;
                    view.branchID = p.BranchID;
                    view.pastorName = p.PastorName;
                    view.Date = Convert.ToDateTime(p.Date);
                    view.StartTime = Convert.ToDateTime(p.StartTime);
                    view.EndTime = Convert.ToDateTime(p.EndTime);
                    view.content = p.content;
                    view.Description = p.Description;
                    view.StartDate = p.StartDate;
                    view.EndDate = p.EndDate;
                }
                return view;
            }


        }
    }
}
