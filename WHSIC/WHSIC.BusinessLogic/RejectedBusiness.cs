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
    public class RejectedBusiness
    {
        public List<RejectedViewModel> GetAllRejectedUpload()
        {
            using (var pr = new RejectedUploadsRepository())
            {
                return pr.getAll().Select(x => new RejectedViewModel()
                {
                    ProgramId = x.ProgramId,
                    Name = x.Name,
                    StartDate = Convert.ToDateTime(x.StartDate),
                    EndDate = Convert.ToDateTime(x.EndDate),
                    Content = x.Content,
                    Venue = x.Venue,
                    Image = x.ImageData,
                    comment=x.comment

                }).ToList();

            }
        }

        public void AddRejectedUpload(TempUploadViewModel model)
        {
            using (var pr = new RejectedUploadsRepository())
            {
                var p = new Rejected
                {
                    Name = model.Name,
                    StartDate = model.StartDate.ToShortDateString(),
                    EndDate = model.EndDate.ToShortDateString(),
                    Content = model.Content,
                    Venue = model.Venue,
                    ImageData = model.Image,
                    comment=model.comment
                    
                };
                pr.Save(p);
            }
        }

        public void DeleteRejectedUpload(int id)
        {
            using (var pr = new RejectedUploadsRepository())
            {
                Rejected p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public RejectedViewModel GetbyID(int id)
        {
            using (var pr = new RejectedUploadsRepository())
            {
                Rejected p = pr.GetByID(id);
                var view = new RejectedViewModel();

                if (p != null)
                {
                    view.Name = p.Name;
                    view.StartDate = Convert.ToDateTime(p.StartDate);
                    view.EndDate = Convert.ToDateTime(p.EndDate);
                    view.Content = p.Content;
                    view.Venue = p.Venue;
                    view.Image = p.ImageData;
                    view.comment = p.comment;
                    view.ProgramId = p.ProgramId;
                }
                return view;
            }


        }
        public void UpdateRejectedUpload(RejectedViewModel model)
        {
            using (var pr = new RejectedUploadsRepository())
            {
                Rejected p = pr.GetByID(model.ProgramId);

                if (p != null)
                {
                    p.Name = model.Name;
                    p.StartDate = model.StartDate.ToShortDateString();
                    p.EndDate = model.EndDate.ToShortDateString();
                    p.Content = model.Content;
                    p.Venue = model.Venue;
                    p.ImageData = model.Image;

                    pr.Update(p);
                }

            }
        }
    }
}

