using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHSIC.Data
{
    public class EventTables
    {

    }

    public class Event
    {
        [Key]
        public int EventID { get; set; }
        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }
        [Required]
        [Display(Name = "Venue/Location")]
        public string Venue { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Discription { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public string StartTime { get; set; }
        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public string EndTime { get; set; }
        public bool Invite { get; set; }

        public bool postponed { get; set; }

        public ICollection<EventGuest> EventGuests { get; set; }
    }
    public class Guest
    {
        [Key]
        public int GuestID { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Guestemail { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact length must be equal to 10 digits ")]
        [Display(Name = "Contact")]
        public string Contact { get; set; }
        [Display(Name = "Guest Type")]
        public string GuestType { get; set; }
        public ICollection<EventGuest> EventGuest { get; set; }


    }
    public class EventGuest
    {
        [Key]
        public int EventGuestID { get; set; }
        [Display(Name = "Guest")]
        public int GuestID { get; set; }
        [Display(Name = "Event")]
        public int EventID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Replydate { get; set; }
        [Display(Name = "Posponed Atendence")]
        public bool Postponed { get; set; }
        public bool Reminder { get; set; }

        public virtual Event Events { get; set; }
        public virtual Guest Guests { get; set; }
    }
}
