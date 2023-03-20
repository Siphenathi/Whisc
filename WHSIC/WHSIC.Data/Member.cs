using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Member
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string id_number { get; set; }
        public int BranchID { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string contact { get; set; }
        public string employed { get; set; }
        public string marital_status { get; set; }
        public string gender { get; set; }
        public string home_address { get; set; }
        public string postal_address { get; set; }
        public string next_of_kin { get; set; }
        public string kin_contact { get; set; }
        public virtual Branch branches { get; set; }


    }
}
