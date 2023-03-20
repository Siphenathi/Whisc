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
    public class ProjectBusiness
    {

        public List<ProjectViewModel> GetAllProjects()
        {
            using (var pr = new ProjectRepository())
            {
                return pr.getAll().Select(x => new ProjectViewModel()
                {
                    ProjectID =x.ProjectID,
                    ProjectName=x.ProjectName,
                    Project_Manager=x.Project_Manager,
                    Project_Description=x.Project_Description,
                    StartDate=x.StartDate,
                    DurationFormat=x.DurationFormat,
                    Duration=x.Duration,
                    EndDate=x.EndDate,
                    PhysicalAddress=x.PhysicalAddress,
                    Contact=x.Contact,
                    Finished=x.Finished,
                    Amount=x.Amount
                }).ToList();

            }
        }

        public void AddProject(ProjectViewModel model)
        {
            using (var pr = new ProjectRepository())
            {
                var p = new Project
                {
                    ProjectName = model.ProjectName,
                    PhysicalAddress = model.PhysicalAddress,
                    Duration = model.Duration,
                    DurationFormat = model.DurationFormat,
                    Project_Manager = model.Project_Manager,
                    Project_Description = model.Project_Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Contact = model.Contact,
                    Finished = false,
                    Amount = 0
                };
                pr.Save(p);
            }
        }

        public void DeleteProject(int id)
        {
            using (var pr = new ProjectRepository())
            {
                Project p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public ProjectViewModel GetbyID(int id)
        {
            using (var pr = new ProjectRepository())
            {
                Project p = pr.GetByID(id);
                var view = new ProjectViewModel();

                if (p != null)
                {
                    view.ProjectID = p.ProjectID;
                    view.ProjectName = p.ProjectName;
                    view.Project_Manager = p.Project_Manager;
                    view.Project_Description = p.Project_Description;
                    view.StartDate = p.StartDate;
                    view.EndDate = p.EndDate;
                    view.DurationFormat = p.DurationFormat;
                    view.Duration = p.Duration;
                    view.Contact = p.Contact;
                    view.PhysicalAddress = p.PhysicalAddress;
                    view.Amount = p.Amount;
                }
                return view;
            }


        }
        public void UpdateProject(ProjectViewModel model)
        {
            using (var pr = new ProjectRepository())
            {
                Project p = pr.GetByID(model.ProjectID);

                if (p != null)
                {
                    p.Project_Description = model.Project_Description;
                    p.StartDate = model.StartDate;
                    p.EndDate = model.EndDate;
                    p.PhysicalAddress = model.PhysicalAddress;
                    p.Contact = model.Contact;
                    p.Duration = model.Duration;
                    p.DurationFormat = model.DurationFormat;
                    p.ProjectName = model.ProjectName;
                    p.Project_Manager = model.Project_Manager;
                    p.Amount = model.Amount;
                    p.Finished = true;
                    pr.Update(p);

                }

            }

        }
    }
}
