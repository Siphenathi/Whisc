using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class PaidExpenseReceipt
    {
        [Key]
        public int Id { get; set; }
        public int paid_exp_id { get; set; }
        public DateTime Date { get; set; }
        public byte[] Receipt { get; set; }
        public virtual  Expense_Paid Expense_Paid { get; set; }
    }
}
