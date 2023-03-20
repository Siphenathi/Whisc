using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class ProjectInvoice
    {
        [Key]
        public int InvoiceID { get; set; }
        public int ProjectID { get; set; }
        public byte[] Invoice { get; set; }
        public double Amount { get; set; }
        public virtual Project Project { get; set; }
    }
}
