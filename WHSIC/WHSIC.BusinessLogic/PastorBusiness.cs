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
    public class PastorBusiness
    {
        public List<PastorViewModel> GetAllPastors()
        {
            using (var pr = new PastorRepository())
            {
                return pr.getAll().Select(x => new PastorViewModel()
                {
                    pastorID=x.pastorID,
                    FirstName=x.PastorFullName.Substring(0,x.PastorFullName.IndexOf(" ")),
                    Surname=x.PastorFullName.Substring(x.PastorFullName.IndexOf(" ")+1),
                    Date = Convert.ToDateTime(x.Date),
                    email=x.Email,
                    contact=x.Contact,
                    image=x.Image,
                    Inside=x.Inside
                    

                }).ToList();

            }
        }

        public void AddPastor(PastorViewModel model)
        {
            using (var pr = new PastorRepository())
            {
                var p = new Pastor
                {
                    PastorFullName = model.FirstName + " " + model.Surname,
                    Date = model.Date.ToShortDateString(),
                    Image=model.image,
                    Contact=model.contact,
                    Email=model.email,
                    Inside=model.Inside
                    
                };
                pr.Save(p);
            }
        }

        public void DeletePastor(int id)
        {
            using (var pr = new PastorRepository())
            {
                Pastor p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public PastorViewModel GetbyID(int id)
        {
            using (var pr = new PastorRepository())
            {
                Pastor p = pr.GetByID(id);
                var view = new PastorViewModel();

                if (p != null)
                {
                    view.FirstName = p.PastorFullName.Substring(0, p.PastorFullName.IndexOf(" "));
                    view.Surname = p.PastorFullName.Substring(p.PastorFullName.IndexOf(" ") + 1);
                    view.Date = Convert.ToDateTime(p.Date);
                    view.image = p.Image;
                    view.contact = p.Contact;
                    view.email = p.Email;
                    view.Inside = p.Inside;
                }
                return view;
            }


        }
        public void UpdatePastor(PastorViewModel model)
        {
            using (var pr = new PastorRepository())
            {
                Pastor p = pr.GetByID(model.pastorID);

                if (p != null)
                {
                    p.PastorFullName = model.FirstName + " " + model.Surname;
                    p.Email = model.email;
                    p.Contact = model.contact;
                    p.Image = model.image;
                    p.Date = model.Date.ToString();
                    p.Inside = model.Inside;

                    pr.Update(p);
                }

            }

        }
    }
}
