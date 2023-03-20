using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public  class Salary
    {
        [Key]
        public int SalaryID { get; set; }
        public int EmployeeNo { get; set; }

        public DateTime Date { get; set; }
        public decimal Net { get; set; }
        public int Tax { get; set; }
        public decimal Gross { get; set; }

        public  virtual Employee Employees { get; set; }
    }
}
