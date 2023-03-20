using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Project_Manager { get; set; }
        public string Project_Description { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public string DurationFormat { get; set; }
        public DateTime EndDate { get; set; }
        public string PhysicalAddress { get; set; }
        public string Contact{ get; set; }
        public bool Finished { get; set; }
        public decimal Amount { get; set; }
        public ICollection<ProjectConstructor> ProjectConstructors { get; set; }
        public ICollection<ProjectProgress> ProjectProgress { get; set; }
        public ICollection <ProjectInvoice> ProjectInvoice { get; set; }
    }
}
