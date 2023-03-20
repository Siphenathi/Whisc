using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }
        public int pastorID { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Pastor Pastors { get; set; }
    }
}
