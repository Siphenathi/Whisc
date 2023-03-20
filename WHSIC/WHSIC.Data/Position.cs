using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Position
    {
        [Key]
        public int PositionID { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<FullTime>FullTime { get; set; }
        public virtual ICollection<PartTime> PartTimes { get; set; }

    }
}
