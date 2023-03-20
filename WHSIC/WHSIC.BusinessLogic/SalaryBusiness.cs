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
    public class SalaryBusiness
    {
        DonorTypeBusiness dt = new DonorTypeBusiness();
        DonorBusiness db = new DonorBusiness();
        EmployeeBusiness eb = new EmployeeBusiness();
        public List<SalaryViewModel> GetAllSalary()
        {
            using (var pr = new SalaryRepository())
            {
                return pr.getAll().Select(x => new SalaryViewModel()
                {
                    SalaryID=x.SalaryID,
                    EmployeeNo=x.EmployeeNo,
                    FirsName=x.Employees.FirstName,
                    LastName=x.Employees.LastName,
                    Date=x.Date,
                    Net=x.Net,
                    Tax=x.Tax,
                    Salary=x.Gross
                }).ToList();

            }
        }
        
        public int tax_no(int empno)
        {
            int num = 0;
            EmployeeViewModel e = eb.GetbyID(empno);
            num = Convert.ToInt32(e.Tax_No);
            return num;

        }
    
        public void AddSalary(SalaryViewModel model)
        {
            using (var pr = new SalaryRepository())
            {
                var z = new Salary
                {
                    Date = model.Date,
                    EmployeeNo = model.EmployeeNo,
                    Gross = model.Salary,
                    Tax = tax_no(model.EmployeeNo),
                    Net = model.Salary
                };
                pr.Save(z);
            }
        }
        public void DeleteSalary(int id)
        {
            using (var pr = new SalaryRepository())
            {
                Salary p = pr.GetByID(id);

                if (p != null)
                {
                    pr.Delete(p);
                }
            }

        }
        public SalaryViewModel GetbyID(int id)
        {
            using (var pr = new SalaryRepository())
            {
                Salary p = pr.GetByID(id);
                var view = new SalaryViewModel();

                if (p != null)
                {
                    view.SalaryID = p.SalaryID;
                    view.EmployeeNo = p.EmployeeNo;
                    view.FirsName = p.Employees.FirstName;
                    view.LastName = p.Employees.LastName;
                    view.Date = p.Date;
                    view.Net = p.Net;
                    view.Tax = p.Tax;
                    view.Salary = p.Gross;
                }
                return view;
            }
        }
        public void UpdateSalary(SalaryViewModel model)
        {
            using (var pr = new SalaryRepository())
            {
                Salary p = pr.GetByID(model.SalaryID);

                if (p != null)
                {
                   
                    p.EmployeeNo = model.EmployeeNo;
                    p.Date = model.Date;
                    p.Net = model.Salary;
                    p.Gross = model.Salary;

                    pr.Update(p);

                }

            }

        }
    }
}
