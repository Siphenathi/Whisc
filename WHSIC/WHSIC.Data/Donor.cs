using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Donor
    {
        [Key]
        public int Donor_ID { get; set; }
        public int DonortypeID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string tax_no { get; set; }
        public byte[] Image { get; set; }
        public virtual DonorType DonorType { get; set; }
        public virtual ICollection<Donation> Donation { get; set; }
    }
}
