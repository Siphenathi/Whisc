using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Sermon
    {
        [Key]
        public int ID { get; set; }
        public int BranchID { get; set; }
        public string PastorName { get; set; }
        public string Date { get; set; }
        public string content { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public virtual Branch Branches { get; set; }
      
    }
}
