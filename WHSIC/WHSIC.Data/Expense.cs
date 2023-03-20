using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Expense
    {
        [Key]
        public int exp_id { get; set; }
        public string desc { get; set; }
        public string payment_due_date { get; set; }
        public string exp_acc_no { get; set; }
        public string from_acc_no { get; set; }
        public decimal amount { get; set; }
        public bool recurring { get; set; }
        public virtual ICollection<Expense_Paid> Expense_Paids { get; set; }
    }
}
