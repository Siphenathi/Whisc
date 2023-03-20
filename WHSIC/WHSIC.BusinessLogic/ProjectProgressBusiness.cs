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
    public class ProjectProgressBusiness
    {
        public List<ProjectProgressViewModel> GetAllProjectProgresses()
        {
            using (var pr = new ProjectProgressRepository())
            {
                return pr.getAll().Select(x => new ProjectProgressViewModel()
                {
                    ProjectID = x.ProjectID,
                    ProgressID=x.ProgressID,
                    Date=x.Date,
                    Duration=x.Duration,
                    PeroidWorked=x.PeroidWorked
                }).ToList();

            }
        }

        public void AddProjectProgresses(ProjectProgressViewModel model)
        {
            using (var pr = new ProjectProgressRepository())
            {
                var p = new ProjectProgress
                {
                    ProjectID=model.ProjectID,
                    Date=model.Date,
                    Duration=model.Duration,
                    PeroidWorked=model.PeroidWorked
                };
                pr.Save(p);
            }
        }

        public void DeleteProjectProgresses(int id)
        {
            using (var pr = new ProjectProgressRepository())
            {
                ProjectProgress p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }
        }

        public ProjectProgressViewModel GetbyID(int id)
        {
            using (var pr = new ProjectProgressRepository())
            {
                ProjectProgress p = pr.GetByID(id);
                var view = new ProjectProgressViewModel();

                if (p != null)
                {
                    view.ProjectID = p.ProjectID;
                    view.ProgressID = p.ProgressID;
                    view.Duration = p.Duration;
                    view.PeroidWorked = p.PeroidWorked;
                    view.Date = p.Date;
                }
                return view;
            }
        }
        public void UpdateProjectProgresses(ProjectProgressViewModel model)
        {
            using (var pr = new ProjectProgressRepository())
            {
                ProjectProgress p = pr.GetByID(model.ProjectID);

                if (p != null)
                {
                    p.Date = model.Date;
                    p.Duration = model.Duration;
                    p.PeroidWorked = model.PeroidWorked;
                    p.ProjectID = model.ProjectID;
                    pr.Update(p);

                }

            }

        }
    }
}
