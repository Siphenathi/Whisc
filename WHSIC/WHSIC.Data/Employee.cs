using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Employee
    {
        [Key]
        public int EmployeeNo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Contact { get; set; }

        public string Email { get; set; }

        public string BankName { get; set; }

        public string AccountNumber { get; set; }

        public string Address { get; set; }

        public int PositionID { get; set; }

        public string Status { get; set; }

        public string Date { get; set; }
        public string Tax_No { get; set; }
        public virtual Position Positions { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        
    }
}
