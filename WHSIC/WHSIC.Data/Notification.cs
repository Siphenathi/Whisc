using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Notification
    {
        [Key]
        public int NotID { get; set; }
        public string Text { get; set; }
        public DateTime DateTime{get;set;}
        public bool Red {get; set;}
    }
}
