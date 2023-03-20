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
    public class ProjectConstructorsBusiness
    {
        public List<ProjectConstructorViewModel> GetAllProjectonstructors()
        {
            using (var pr = new ProjectConstructorsRepository())
            {
                return pr.getAll().Select(x => new ProjectConstructorViewModel()
                {
                    ContructorID=x.ContructorID,
                    ProjectID=x.ProjectID,
                    Constructor_Name=x.Constructor_Name,
                    email=x.email,
                    Address=x.Address,
                    Contact=x.Contact

                }).ToList();

            }
        }

        public void AddProjectConstructor(ProjectConstructorViewModel model)
        {
            using (var pr = new ProjectConstructorsRepository())
            {
                var p = new ProjectConstructor
                {
                    ProjectID=model.ProjectID,
                    Constructor_Name=model.Constructor_Name,
                    Contact=model.Contact,
                    email=model.email,
                    Address=model.Address
                };
                pr.Save(p);
            }
        }

        public void DeleteProjectonstructor(int id)
        {
            using (var pr = new ProjectConstructorsRepository())
            {
                ProjectConstructor p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }
        }

        public ProjectConstructorViewModel GetbyID(int id)
        {
            using (var pr = new ProjectConstructorsRepository())
            {
                ProjectConstructor p = pr.GetByID(id);
                var view = new ProjectConstructorViewModel();

                if (p != null)
                {
                    view.ProjectID = p.ProjectID;
                    view.ContructorID = p.ContructorID;
                    view.Constructor_Name = p.Constructor_Name;
                    view.Contact = p.Contact;
                    view.email = p.email;
                    view.Address = p.Address;
                }
                return view;
            }
        }
        public void UpdateProjectonstructor(ProjectConstructorViewModel model)
        {
            using (var pr = new ProjectConstructorsRepository())
            {
                ProjectConstructor p = pr.GetByID(model.ContructorID);

                if (p != null)
                {
                    p.Contact = model.Contact;
                    p.email = model.email;
                    p.Address = model.Address;
                    p.Constructor_Name = model.Constructor_Name;

                    pr.Update(p);
                }
            }
        }
    }
}
