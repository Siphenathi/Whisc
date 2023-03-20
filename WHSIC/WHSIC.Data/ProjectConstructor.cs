using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class ProjectConstructor
    {
        [Key]
        public int ContructorID { get; set; }
        public int ProjectID { get; set; }
        public string Constructor_Name { get; set; }
        public string email { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public virtual Project Project { get; set; }
    }
}
