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
    public class VisitorsBusiness
    {
        public List<VisitorCountsViewModel> GetAllVisitors()
        {
            using (var pr = new VisitorRepository())
            {
                return pr.getAll().Select(x => new VisitorCountsViewModel()
                {
                    Id = x.Id,
                    Date = x.Date,
                    IpAddress = x.IpAddress
                }).ToList();

            }
        }

        public void AddVisitor(VisitorCountsViewModel model)
        {
            using (var pr = new VisitorRepository())
            {
                var p = new VisitorsCount
                {
                    Date = model.Date,
                    IpAddress = model.IpAddress
                };
                pr.Save(p);
            }
        }

        public void UpdateVisitor(VisitorCountsViewModel model)
        {
            using (var pr = new VisitorRepository())
            {
                VisitorsCount p = pr.getAll().Find(a => a.IpAddress == model.IpAddress);
                if (p != null)
                {
                    p.Date = model.Date;
                    pr.Update(p);
                }

            }
        }
    }
}
