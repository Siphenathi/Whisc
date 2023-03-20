using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class FullTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Basic {get;set;}
        public decimal BasicRate { get; set; }
        public int PositionID { get; set; }
         
        public virtual Position Positions { get; set; }
    }
}
