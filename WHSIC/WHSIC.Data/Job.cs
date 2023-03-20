using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }
        [Required]
        [Display(Name = "Job Position ")]
        public string Position { get; set; }
        [Required]
        [Display(Name = "Requirements ")]
        public string Requirements { get; set; }
        [Required]
        [Display(Name = "Duties ")]
        public string Duties { get; set; }
        [Required]
        [Display(Name = "Closing Date ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ClosingDate { get; set; }
        [Display(Name = "Salary")]
        [Required]
        public double Salary { get; set; }
        [Display(Name = "Position Status ")]
        [Required]
        public string PositionStatus { get; set; }
        [Display(Name = "How to Apply ")]
        [Required]
        public string ApplyingInfo { get; set; }
    }
}
