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
    public class SermonBusiness
    {
        public List<SermonViewModel> GetAllSermons()
        {
            using (var pr = new SermonRepository())
            {
                return pr.getAll().Select(x => new SermonViewModel()
                {
                    ID=x.ID,
                    Date=Convert.ToDateTime(x.Date),
                    content=x.content,
                    StartTime=Convert.ToDateTime(x.StartTime),
                    EndTime= Convert.ToDateTime(x.EndTime),
                    pastorName=x.PastorName,
                    BranchName = x.Branches.BranchName,
                    branchID = x.BranchID
               
                }).ToList();

            }
        }
        
        public void AddSermon(SermonViewModel model)
        {
            using (var pr = new SermonRepository())
            {
                var p = new Sermon
                {
                    Date = model.Date.ToShortDateString(),
                    content=model.content,
                    StartTime=model.StartTime.ToShortTimeString(),
                    EndTime=model.StartTime.ToShortTimeString(),
                    PastorName=model.pastorName,
                    BranchID = model.branchID
                };
                pr.Save(p);
            }
        }

        public void DeleteSermon(int id)
        {
            using (var pr = new SermonRepository())
            {
                Sermon p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public SermonViewModel GetbyID(int id)
        {
            using (var pr = new SermonRepository())
            {
                Sermon p = pr.GetByID(id);
                var view = new SermonViewModel();

                if (p != null)
                {
                    view.ID = p.ID;
                    view.branchID=p.BranchID;
                    view.BranchName = p.Branches.BranchName;
                    view.pastorName = p.PastorName;
                    view.Date = Convert.ToDateTime(p.Date);
                    view.StartTime = Convert.ToDateTime(p.StartTime);
                    view.EndTime = Convert.ToDateTime(p.EndTime);
                    view.content = p.content;
                }
                return view;
            }


        }
        public void UpdateSermon(SermonViewModel model)
        {
            using (var pr = new SermonRepository())
            {
                Sermon p = pr.GetByID(model.ID);

                if (p != null)
                {
                    p.Date = model.Date.ToShortDateString();
                    p.content = model.content;
                    p.StartTime = model.StartTime.ToShortTimeString();
                    p.EndTime = model.EndTime.ToShortTimeString();
                    p.PastorName = model.pastorName;
                    p.BranchID = p.BranchID;

                    pr.Update(p);
                }

            }

        }
    }
}
