using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public  class Donation
    {
        [Key]
        public int DonationID { get; set; }
        public int Donor_ID { get; set; }
        public int DonationtypeID { get; set; }

        public string Payment_Method { get; set; }

        public string Date { get; set; }

        public decimal Amount { get; set; }
        public virtual DonationType DonationTypes { get; set; }
        public virtual Donor Donor { get; set; }
    }
}
