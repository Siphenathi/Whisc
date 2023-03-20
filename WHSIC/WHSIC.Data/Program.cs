using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Program
    {
        [Key]
        public int PId { get; set; }
        public string PName { get; set; }
        public string Date { get; set;}
        public string Frequence { get; set; }

        public string Venue { get; set; }
    }
}
