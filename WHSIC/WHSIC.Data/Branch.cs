using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Branch
    {
        [Key]
        public int BranchID { get; set; }
        public int pastorID { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string Date { get; set; }
        public string Contact { get; set; }
        public string Capacity { get; set; }

        public virtual ICollection<Member> Members { get; set; }
        public ICollection<Sermon> Sermons { get; set; } 
        public ICollection<FinalSermon> FinalSermons { get; set; }
        public virtual Pastor Pastors { get; set; }

    }
}
