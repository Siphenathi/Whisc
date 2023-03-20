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
    public class ProjectInvoicesBusiness
    {
        public List<ProjectInvoiceViewModel> GetAllProjectInvoices()
        {
            using (var pr = new ProjectInvoiceRepository())
            {
                return pr.getAll().Select(x => new ProjectInvoiceViewModel()
                {
                    ProjectID=x.ProjectID,
                    InvoiceID=x.InvoiceID,
                    Invoice=x.Invoice,
                    Amount=x.Amount

                }).ToList();
            }
        }
        public void AddProjectInvoice(ProjectInvoiceViewModel model)
        {
            using (var pr = new ProjectInvoiceRepository())
            {
                var p = new ProjectInvoice
                {
                    ProjectID = model.ProjectID,
                    Invoice = model.Invoice,
                    Amount = model.Amount
                };
                pr.Save(p);
            }
        }

        public void DeleteProjectInvoice(int id)
        {
            using (var pr = new ProjectInvoiceRepository())
            {
                ProjectInvoice p = pr.GetByID(id);

                if(p != null)
                {
                    pr.Delete(p);
                }
            }
        }

        public ProjectInvoiceViewModel GetbyID(int id)
        {
            using (var pr = new ProjectInvoiceRepository())
            {
                ProjectInvoice p = pr.GetByID(id);
                var view = new ProjectInvoiceViewModel();

                if (p != null)
                {
                    view.ProjectID = p.ProjectID;
                    view.Amount = p.Amount;
                    view.InvoiceID = p.InvoiceID;
                    view.Invoice = p.Invoice;
                }
                return view;
            }
        }
        public void UpdateProjecInvoice(ProjectInvoiceViewModel model)
        {
            using (var pr = new ProjectInvoiceRepository())
            {
                ProjectInvoice p = pr.GetByID(model.ProjectID);

                if (p != null)
                {
                    p.ProjectID = model.ProjectID;
                    p.Invoice = model.Invoice;
                    p.InvoiceID = model.InvoiceID;
                    p.Amount = model.Amount;

                    pr.Update(p);
                }
            }
        }

    }
}
