using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class VisitorsCount
    {
        [Key]
        public int Id { get; set; }
        public string IpAddress {get;set;}
        public DateTime Date { get; set; }
    }
}
