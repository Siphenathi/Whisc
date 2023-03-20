using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class ServiceType
    {
        [Key]
        public int TypeID { get; set; }
        public string Description { get; set; }
        public bool dismiss  { get; set; }
        public ICollection<Servicee>Service { get; set; }
    }
}
