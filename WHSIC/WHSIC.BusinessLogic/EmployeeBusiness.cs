using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WHSIC.Data;
using WHSIC.Model;
using WHSIC.Service;

namespace WHSIC.BusinessLogic
{
    public class EmployeeBusiness
    {
        PositionBusiness pb = new PositionBusiness();
        public string Description(int PositionID)
        {
            PositionViewModel pv = pb.GetbyID(PositionID);
            return pv.Description;
        }
        public List<EmployeeViewModel> GetAllEmployee()
        {
            using (var pr = new EmployeeRepository())
            {
                return pr.getAll().Select(x => new EmployeeViewModel()
                {
                    EmployeeNo = x.EmployeeNo,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Contact = x.Contact,
                    Email = x.Email,
                    BankName = x.BankName,
                    AccountNumber = x.AccountNumber,
                    Address = x.Address,
                    PositionID = x.PositionID,
                    Description = Description(x.PositionID),
                    Status=x.Status,
                    Date=Convert.ToDateTime(x.Date),
                    Tax_No=x.Tax_No

                }).ToList();

            }
        }

        public void AddEmployee(EmployeeViewModel model)
        {
            using (var pr = new EmployeeRepository())
            {
                var p = new Employee
                {
                    FirstName=model.FirstName,
                    LastName = model.LastName,
                    Contact=model.Contact,
                    Email=model.Email,
                    BankName=model.BankName,
                    AccountNumber=model.AccountNumber,
                    Address=model.Address,
                    PositionID=model.PositionID,
                    Status=model.Status,
                    Date=model.Date.ToShortDateString(),
                    Tax_No=model.Tax_No.ToString()
                };
                pr.Save(p);
            }
          

        }

        public void DeleteEmployee(int id)
        {
            using (var pr = new EmployeeRepository())
            {
                Employee p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }

        public EmployeeViewModel GetbyID(int id)
        {
            using (var pr = new EmployeeRepository())
            {
                Employee p = pr.GetByID(id);
                var view = new EmployeeViewModel();

                if (p != null)
                {
                    view.EmployeeNo = p.EmployeeNo;
                    view.FirstName = p.FirstName;
                    view.LastName = p.LastName;
                    view.Contact = p.Contact;
                    view.Email = p.Email;
                    view.BankName = p.BankName;
                    view.AccountNumber = p.AccountNumber;
                    view.Address = p.Address;
                    view.PositionID = p.PositionID;
                    view.Description = Description(p.PositionID);
                    view.Status = p.Status;
                    view.Date = Convert.ToDateTime(p.Date);
                    view.Tax_No =p.Tax_No;
                }
                return view;
            }


        }
        public void UpdateEmployee(EmployeeViewModel model)
        {
            using (var pr = new EmployeeRepository())
            {
                Employee p = pr.GetByID(model.PositionID);

                if (p != null)
                {
                    p.FirstName = model.FirstName;
                    p.LastName = model.LastName;
                    p.Contact = model.Contact;
                    p.Email = model.Email;
                    p.BankName = model.BankName;
                    p.AccountNumber = model.AccountNumber;
                    p.Address = model.Address;
                    p.Tax_No = model.Tax_No.ToString();
                    p.Date = model.Date.ToString();
                    p.PositionID = model.PositionID;
                    p.Status = model.Status;

                    pr.Update(p);
                }

            }

        }
    }
}
