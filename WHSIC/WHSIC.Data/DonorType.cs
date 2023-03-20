using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class DonorType
    {
        [Key]
        public int DonortypeID { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Donor> Donors { get; set;}
    }
}
