using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Servicee
    {
        [Key]
        public int Service_id { get; set; }
        public int TypeID { get; set; }
        public string Service_name { get; set; }        
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public string Donor_name { get; set; }
        public virtual ServiceType ServiceTypes { get; set; }
    }
}
