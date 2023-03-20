using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public  class DonationType
    {
        [Key]
        public int DonationtypeID { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Donation> Donations { get; set; }
    }
}
