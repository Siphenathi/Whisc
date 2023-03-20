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
    public class TempBusiness
    {
        Email obj = new Email();
        SMS sm = new SMS();
        public List<TempUploadViewModel> GetAllTempUpload()
        {
            using (var pr = new TempUploadRepository())
            {
                return pr.getAll().Select(x => new TempUploadViewModel()
                {
                    ProgramId = x.ProgramId,
                    Name = x.Name,
                    StartDate = Convert.ToDateTime(x.StartDate),
                    EndDate = Convert.ToDateTime(x.EndDate),
                    Content = x.Content,
                    Venue = x.Venue,
                    Image = x.ImageData

                }).ToList();

            }
        }


        public void AddTempUpload(ProgramUploadViewModel model)
        {
            using (var pr = new TempUploadRepository())
            {
                var p = new TempUpload
                {
                    Name = model.Name,
                    StartDate = model.StartDate.ToShortDateString(),
                    EndDate = model.EndDate.ToShortDateString(),
                    Content = model.Content,
                    Venue = model.Venue,
                    ImageData = model.Image
                };
                pr.Save(p);
                //Send email Synchronously
                //obj.to = new MailAddress("spantshwa.lukho@gmail.com");

                //obj.body = "Hi " + "spantshwa.lukho@gmail.com" + "Secretary has uploaded program images and details but, the process needs your confirmation inorder to be done successfully";
                //f +=  "and " + obj.ConfirmUploads();

                // Send SMS
                string n = "0780329830";
                sm.sending_full_sms(n, " Hi " + "Bishop , Secretary has uploaded program contents that require your confirmation inorder to be successfully uploaded");

                //await MailService.SendMail("21426379@dut4life.ac.za", "Contents to be confirmed"," Hi " + "Bishop , Secretary has uploaded program contents that require your confirmation in order to be successfully uploaded");
            }
        }

        public void RestoreUpload(RejectedViewModel model)
        {
            using (var pr = new TempUploadRepository())
            {
                var p = new TempUpload
                {
                    Name = model.Name,
                    StartDate = model.StartDate.ToShortDateString(),
                    EndDate = model.EndDate.ToShortDateString(),
                    Content = model.Content,
                    Venue = model.Venue,
                    ImageData = model.Image
                };
                pr.Save(p);
                //string n = "0780329830";
               // sm.sending_full_sms(n, " Hi " + "Bishop , Secretary has uploaded program contents that require your confirmation inorder to be successfully uploaded");
            }
        }
        public void DeleteTempUpload(int id)
        {
            using (var pr = new TempUploadRepository())
            {
                TempUpload p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public TempUploadViewModel GetbyID(int id)
        {
            using (var pr = new TempUploadRepository())
            {
                TempUpload p = pr.GetByID(id);
                var view = new TempUploadViewModel();

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
        public void UpdateTempUpload(TempUploadViewModel model)
        {
            using (var pr = new TempUploadRepository())
            {
                TempUpload p = pr.GetByID(model.ProgramId);

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
