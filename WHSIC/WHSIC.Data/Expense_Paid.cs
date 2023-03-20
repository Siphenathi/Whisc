using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Expense_Paid
    {
        [Key]
        public int paid_exp_id { get; set; }
        public int exp_id { get; set; }

        public DateTime date_paid { get; set; }

        public decimal amount_paid { get; set; }
        public string Reference { get; set;}
        public virtual Expense Expences { get; set; }
        public ICollection<PaidExpenseReceipt> PaidExpenseReceipt { get; set; }
    }
}
