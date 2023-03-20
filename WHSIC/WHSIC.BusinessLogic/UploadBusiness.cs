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
    public class UploadBusiness
    {
        public List<ProgramUploadViewModel> GetAllProgramUpload()
        {
            using (var pr = new UploadRepository())
            {
                return pr.getAll().Select(x => new ProgramUploadViewModel()
                {
                    ProgramId=x.ProgramId,
                    Name=x.Name,
                    StartDate=Convert.ToDateTime(x.StartDate),
                    EndDate= Convert.ToDateTime(x.EndDate),
                    Content=x.Content,
                    Venue = x.Venue,
                    Image=x.ImageData

                }).ToList();

            }
        }

        public void AddProgramUpload(TempUploadViewModel model)
        {
            using (var pr = new UploadRepository())
            {
                var p = new ProgramUpload
                {
                    Name = model.Name,
                    StartDate = model.StartDate.ToShortDateString(),
                    EndDate = model.EndDate.ToShortDateString(),
                    Content = model.Content,
                    Venue = model.Venue,
                    ImageData = model.Image
                };
                pr.Save(p);
            }
        }

        public void DeleteProgramUpload(int id)
        {
            using (var pr = new UploadRepository())
            {
                ProgramUpload p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public ProgramUploadViewModel GetbyID(int id)
        {
            using (var pr = new UploadRepository())
            {
                ProgramUpload p = pr.GetByID(id);
                var view = new ProgramUploadViewModel();

                if (p != null)
                {
                    view.Name = p.Name;
                    view.StartDate = Convert.ToDateTime(p.StartDate);
                    view.EndDate = Convert.ToDateTime(p.EndDate);
                    view.Content = p.Content;
                    view.Venue = p.Venue;
                    view.Image = p.ImageData;
                }
                return view;
            }


        }
        public void UpdateProgramUpload(ProgramUploadViewModel model)
        {
            using (var pr = new UploadRepository())
            {
                ProgramUpload p = pr.GetByID(model.ProgramId);

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
