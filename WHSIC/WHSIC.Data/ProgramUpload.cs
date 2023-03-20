using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class ProgramUpload
    {
        [Key]
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string  EndDate { get; set; }
        public string Venue { get; set; }
        public string Content { get; set; }
        public byte[] ImageData { get; set; }
    }
}
