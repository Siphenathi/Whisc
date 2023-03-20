using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class ProjectProgress
    {
        [Key]
        public int ProgressID { get; set; }
        public int ProjectID { get; set; }
        public DateTime Date { get; set; }
        public string Duration { get; set; }
        public int PeroidWorked { get; set; }
        public virtual Project Project { get; set; }
    }
}
