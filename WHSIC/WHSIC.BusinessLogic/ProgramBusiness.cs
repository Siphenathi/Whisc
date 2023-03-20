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
    public class ProgramBusiness
    { 
        public List<ProgramViewModel>GetAllProgramies()
        {
            using (var pr = new ProgramRepository())
            {
                return pr.getAll().Select(x => new ProgramViewModel()
                {
                    PId=x.PId,
                    PName=x.PName,
                    Frequence=x.Frequence,
                    Date=Convert.ToDateTime(x.Date),
                    Venue=x.Venue,
                    

                }).ToList();

            }
        }

        public void AddProgram(ProgramViewModel model)
        {
            using (var pr = new ProgramRepository())
            {
                var p = new Program
                {
                    PName = model.PName,
                    Frequence = model.Frequence,
                    Date = model.Date.ToShortDateString(),
                    Venue = model.Venue
                };
                pr.Save(p);
            }
        }

        public void DeleteProgram(int id)
        {
            using (var pr = new ProgramRepository())
            {
                Program p = pr.GetByID(id);

                if(p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public ProgramViewModel GetbyID(int id)
        {
            using (var pr = new ProgramRepository())
            {
                Program p = pr.GetByID(id);
                var view = new ProgramViewModel();

                if(p!=null)
                {
                    view.PName = p.PName;
                    view.Frequence = p.Frequence;
                    view.Date = Convert.ToDateTime(p.Date);
                    view.Venue = p.Venue;
                }
                return view;
            }
           

        }
        public void UpdateProgram(ProgramViewModel model)
        {
            using (var pr =new ProgramRepository())
            {
                Program p = pr.GetByID(model.PId);

                if(p!=null)
                {
                    p.PName = model.PName;
                    p.Date = model.Date.ToShortDateString();
                    p.Venue = model.Venue;
                    p.Frequence = model.Frequence;

                    pr.Update(p);
                }
               
            }
    
        }

      
    }
}
