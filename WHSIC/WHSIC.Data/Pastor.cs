using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Pastor
    {
        [Key]
        public int pastorID { get; set; }
        public string PastorFullName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Date { get; set; }
        public byte [] Image { get; set; }
        public bool Inside { get; set; }

        public ICollection<Branch> Branches { get; set; }
        public ICollection<Availability> Availabililties { get; set; }
    }
}
