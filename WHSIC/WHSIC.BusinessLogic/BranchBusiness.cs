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
    public class BranchBusiness
    {
        public List<BranchViewModel> GetAllBranches()
        {
            using (var pr = new BranchRepository())
            {
                return pr.getAll().Select(x => new BranchViewModel()
                {
                    BranchID = x.BranchID,
                    BranchName=x.BranchName,
                    pastorID = x.pastorID,
                    Address=x.Address,
                    Date = Convert.ToDateTime(x.Date),
                    Contact=x.Contact,
                    Capacity=x.Capacity,
                    PastorName = x.Pastors.PastorFullName
                }).ToList();

            }
        }

        public void AddBranch(BranchViewModel model)
        {
            using (var pr = new BranchRepository())
            {
                var p = new Branch
                {
                    BranchName = model.BranchName,
                    pastorID = model.pastorID,
                    Address = model.Address,
                    Date = model.Date.ToShortDateString(),
                    Contact = model.Contact,
                    Capacity = model.Capacity,
                    
                };
                pr.Save(p);
            }
        }

        public void DeleteBranch(int id)
        {
            using (var pr = new BranchRepository())
            {
                Branch p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public BranchViewModel GetbyID(int id)
        {
            using (var pr = new BranchRepository())
            {
                Branch p = pr.GetByID(id);
                var view = new BranchViewModel();

                if (p != null)
                {
                    view.pastorID = p.pastorID;
                    view.BranchName = p.BranchName;
                    view.Date = Convert.ToDateTime(p.Date);
                    view.Contact = p.Contact;
                    view.Capacity = p.Capacity;
                    view.Address = p.Address;
                    view.PastorName = p.Pastors.PastorFullName;
                    view.BranchID = p.BranchID;
                }
                return view;
            }


        }
        public void UpdateBranch(BranchViewModel model)
        {
            using (var pr = new BranchRepository())
            {
                Branch p = pr.GetByID(model.BranchID);

                if (p != null)
                {
                    p.pastorID = model.pastorID;
                    p.Date =model.Date.ToLongDateString();
                    p.BranchName = model.BranchName;
                    p.Address = model.Address;
                    p.Capacity = model.Capacity;
                    p.Contact = model.Contact;

                    pr.Update(p);
                    
                }

            }

        }
    }
}
