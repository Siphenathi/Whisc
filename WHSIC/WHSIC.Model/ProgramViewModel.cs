using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using WHSIC.Data;
using DHTMLX.Scheduler;

namespace WHSIC.Model
{
    public class MyDonations
    {
        public  List<DonationViewModel> CashDonations { get; set; }
        public List<ServiceeViewModel> Service { get; set; }
    }

    public class Asset_RenterViewModel
    {
        [Key]
        public int Renter_code { get; set; }
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact number length must be equal to 10 digits ")]
        public string Contact { get; set; }
        //[Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [Display(Name = "Physical Address")]
        public string Physical_Address { get; set; }
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
        public byte[] Contract { get; set; }

        public ICollection<Asset_RentalViewModel> Asset_Rentals { get; set; }
        public ICollection<Damaged_AssetViewModel> Damaged_Assets { get; set; }
    }
    public class RegisterRenterModel
    {
        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact number length must be equal to 10 digits ")]
        public string Contact { get; set; }

    }

    public class CodeModel
    {
        [Required]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }
    public class Asset_RentalViewModel
    {
        [Key]
        public int Rental_code { get; set; }
        [Required]
        [Display(Name = "Asset Name")]
        public int Asset_Code { get; set; }
        [Required]
        [Display(Name = "Renter")]
        public int Renter_code { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        //[Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        [Required]
        [Display(Name = "Number of Days")]
        public int Days { get; set; }
        [Display(Name = "Rental Amount")]
        public decimal Amount { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        public int quantity { get; set; }

        [Display(Name = "Returned Quantity")]
        public int returnedQty { get; set; }
        [Display(Name = "Is it Returned ?")]
        public bool Returned { get; set; }
        public int borrowedQty { get; set; }
        public virtual Asset_RenterViewModel Asset_Renters { get; set; }
        public virtual AssetViewModel Asset { get; set; }
    }

    public class Damaged_AssetViewModel
    {
        [Key]
        public int Damaged_ID { get; set; }
        [Required]
        [Display(Name = "Asset Name")]
        public int Asset_Code { get; set; }
        [Required]
        [Display(Name = "Renter Name")]
        public int Renter_code { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PayDate { get; set; }
        public byte[] Quotation { get; set; }
        public virtual Asset_RenterViewModel Asset_Renter { get; set; }
        public virtual AssetViewModel Assets { get; set; }

    }
    public class Asset_CategoryViewModel
    {
        [Key]
        public int CategoryID { get; set; }
        [Required]
        [Display(Name = "Asset Category")]
        public string CName { get; set; }

        [Required]
        [Display(Name = "Asset Type")]
        public string Type { get; set; }
        public ICollection<AssetViewModel> Asset { get; set; }
    }

    public class SupplierViewModel
    {
        [Key]
        public int supply_code { get; set; }
        [Required]
        [Display(Name = "Suppliers Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Contact")]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact length must be equal to 10 digits ")]
        public string contact { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [Display(Name = "Physical Address")]
        public string address { get; set; }
        public ICollection<AssetViewModel> Asset { get; set; }
    }

    public class AssetViewModel
    {
        [Key]
        public int Asset_Code { get; set; }
        [Required]
        [Display(Name = "Suppliers")]
        public int supply_code { get; set; }
        [Required]
        [Display(Name = "Asset Category")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Asset Name")]
        public string AName { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int quantity { get; set; }
        [Required]
        [Display(Name = "Asset Price")]
        public decimal Amount { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }
        [Required]
        [Display(Name = "Life Span")]
        public int LifeSpan { get; set; }
        [Required]
        [Display(Name = "Life Span Format")]
        public string LifeSpanFormat { get; set; }
        [Required]
        [Display(Name = "Warranty")]
        public int Warranty { get; set; }
        [Display(Name = "This asset must be considered for rental")]
        public bool ForRental { get; set; }
        public virtual Asset_CategoryViewModel Asset_Category { get; set; }
        public ICollection<Damaged_AssetViewModel> Damaged_Assets { get; set; }
        public ICollection<AssetChargeViewModel> AssetCharges { get; set; }
        public ICollection<Asset_RentalViewModel> AssetRental { get; set; }
    }
    public class AssetChargeViewModel
    {
        [Key]
        public int charge_ID { get; set; }
        public int Asset_Code { get; set; }
        [Required]
        [Display(Name = "Rental Amount")]
        public decimal Rental_Amount { get; set; }
        [Required]
        [Display(Name = "Penalty Rate(%)")]
        public decimal PenaltyRate { get; set; }
        public string AssetName { get; set; }
        public virtual AssetViewModel Assets { get; set; }
    }
    public class ServiceTypeViewModel
    {
        public int TypeID { get; set; }
        [Required]
        [Display(Name = "Service Type")]
        public string Description { get; set; }
        [Display(Name = "Is the church in need of this type of service?")]
        public bool dismiss { get; set; }
    }
    public class ServiceeViewModel
    {
        public int Service_id { get; set; }
        [Required]
        [Display(Name = "Service Type")]
        public int TypeID { get; set; }
        [Display(Name = "Service Type")]
        public string TypeDesc { get; set; }
        [Required]
        [Display(Name ="Name")]
        public string Service_name { get; set; }
        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public string email { get; set;}

    }
    public class FREportViewModel
    {
        public List<DonationViewModel> Income { get; set; }
        public List<Expense_PaidViewModel> Expense { get; set; }
        public List<Asset_Rentalz> Rental { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class ProjectInvoiceViewModel
    {
        //[Key]
        public int InvoiceID { get; set; }
        public int ProjectID { get; set; }
        public byte[] Invoice { get; set; }
        [Required]
        public double Amount { get; set; }
        //public virtual Project Project { get; set; }
    }
    public class ProjectProgressViewModel
    {
        //[Key]
        public int ProgressID { get; set; }
        public int ProjectID { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required]
        public string Duration { get; set; }
        [Required]
        public int PeroidWorked { get; set; }
    }
    public class ProjectConstructorViewModel
    {
        //[Key]
        public int ContructorID { get; set; }
        [Required]
        public int ProjectID { get; set; }
        [Required]
        [Display(Name ="Constructor Name")]
        public string Constructor_Name { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required]
        [Display(Name = "Phyical Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Contact Number")]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact length must be equal to 10 digits ")]
        public string Contact { get; set; }
        
    }
    public class ProjectViewModel
    {
        //[Key]
        public int ProjectID { get; set; }
        [Display(Name = "Project Name")]
        [Required]
        public string ProjectName { get; set; }
        [Required]
        [Display(Name = "Project Manager")]
        public string Project_Manager { get; set;}
        [Required]
        [Display(Name = "Description")]
        public string Project_Description { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Duration in")]
        public string DurationFormat { get; set; }
        [Required]
        [Display(Name = "Duration")]
        public int Duration { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Physical Address")]
        public string PhysicalAddress { get; set; }
        [Required]
        [Display(Name = "Contact")]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Contact lenght must be equal to 10 digits ")]
        public string Contact { get; set; }
        public decimal Amount { get; set; }
        public bool Finished { get; set; }
    }
    public class RemoveFromRole
    {
        [Required]
        public string Employee { get; set;}
    }
    public class RolesViewModel
    {
        [Required]
        public string Employee {get;set;}
        [Required]
        public string Role { get;set;}
    }
    public class NotificationViewModel
    {
        public int NotID { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public bool Red { get; set; }
    }
    public class VisitorCountsViewModel
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime Date { get; set; }
    }
    public class AlgorithmDatesViewModel
    {
        public int dateID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }

    public class AvailabilityViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Pastor Name")]
        public int pastorID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Startdate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public string PastorName { get; set; }

    }
    public class GalleryViewModel
    {
        public int GalleryId { get; set; }
        public byte[] ImageData { get; set; }
    }
    public class PaidExpenseReceiptViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Expense Name")]
        public string Expense_Name { get; set; }
        public int paid_exp_id { get; set; }
        public DateTime Date { get; set; }
        public byte[] Receipt { get; set; }
    }
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10,MinimumLength = 10,ErrorMessage = "Contact lenght must be equal to 10 digits ")]
        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Comment { get; set; }
    }
    public class MemberViewModel
    {
        [Display(Name = "ID Number")]
        [Required]
        [StringLength(13,MinimumLength =13)]
        public string id_number { get; set; }

        [Display(Name = "Branch Name")]
        public string branchName { get; set; }
        [Required]
        public int BranchID { get; set; }
        [Display(Name = "Name")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Surname")]
        [Required]
        public string surname { get; set; }

        [Display(Name = "Email Address")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Contact")]
        [Required]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10)]
        public string contact { get; set; }

        [Display(Name = "Employment Status")]
        [Required]
        public string employed { get; set; }

        [Display(Name = "Married?")]
        [Required]
        public string marital_status { get; set; }

        [Display(Name = "Gender")]
        [Required]
        public string gender { get; set; }

        [Display(Name = "Home Address")]
        [Required]
        public string home_address { get; set; }

        [Display(Name = "Postal Address")]
        [Required]
        public string postal_address { get; set; }

        [Display(Name = "Next Of Kin")]
        [Required]
        public string next_of_kin { get; set; }

        [Display(Name = "Kin Contact")]
        [Required]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-. ]?([0-9]{8})$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(10)]
        public string kin_contact { get; set; }
    }
    public class SalaryViewModel
    {
        public int SalaryID { get; set; }
        [Required]
        [Display(Name ="Employee")]
        public int EmployeeNo { get; set; }
        public string FirsName { get; set; }
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        public DateTime Date { get; set; }
        public decimal Net { get; set; }
        public int Tax { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
    public class ProgramUploadViewModel
    {
        public int ProgramId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Venue { get; set; }
        [Required]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Content { get; set; }
        public byte[] Image { get; set; }
    }

    public class ProgramViewModel
    {
        public int PId { get; set; }
        [Display(Name = "Program Name")]
        [Required(ErrorMessage = "Please enter Program Name")]

        public string PName { get; set; }

        [Required]
        // [RegularExpression(@"(^[1-9][0-9]{3}$)",ErrorMessage ="Year must be four numeric values")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Days of occurrence")]
        [Required(ErrorMessage = "Please provide Days of occurrence of ")]
        public string Frequence { get; set; }

        [Required]
        public string Venue { get; set; }
    }

    public class ProgramUploadListModel
    {
        public List<ProgramUploadViewModel> ProgramUploadList { get; set; }
    }

    public class ProgramListModel
    {
        public List<ProgramViewModel> ProgramList { get; set; }
    }
    public class PastorViewModel
    {
        [Key]
        public int pastorID { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last name")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required]
        [Display(Name = "Contact")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string contact { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name ="Pastor within the church ?")]
        public bool Inside { get; set; }
        public byte[] image { get; set; }
    }

    public class FinSermonViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DHXJson(Alias = "id")]
        public int ID { get; set; }
        [Required]
        public int branchID { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Content")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string content { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime StartTime { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime EndTime { get; set; }
        [Required]
        public string pastorName { get; set; }

        [DHXJson(Alias = "text")]
        public string Description { get; set; }

        [DHXJson(Alias = "start_date")]
        public DateTime StartDate { get; set; }

        [DHXJson(Alias = "end_date")]
        public DateTime EndDate { get; set; }
    }
    public class SermonViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DHXJson(Alias = "id")]
        public int ID { get; set; }
        [Required]
        public int branchID { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Content")]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string content { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime EndTime { get; set; }
        [Required]
        public string pastorName { get; set; }
        public string BranchName { get; set; }
    }
    public class BranchViewModel
    {
        [Required]
        public string BranchName { get; set; }
        [Required]
        [Display(Name = "Pastor Name")]
        public int pastorID { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string Capacity { get; set; }
        public int BranchID { get; set; }

        public string PastorName { get; set; }
    }
    public class TempUploadViewModel
    {
        public int ProgramId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Venue { get; set; }
        [Required]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public string comment { get; set; }
    }
    public class RejectedViewModel
    {
        public int ProgramId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Venue { get; set; }
        [Required]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,1000}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Content { get; set; }
        public byte[] Image { get; set; }
        public string comment { get; set; }

    }
    public class PositionViewModel
    {
        public int PositionID { get; set; }
        [Required]
        public string Description { get; set; }
    }
    public class FullTimeViewModel
    {
        public int ID { get; set; }
        [Required]
        public decimal BasicRate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Description")]
        public int PositionID { get; set; }

    }

    public class PartTimeViewModel
    {
        public int ID { get; set; }
        [Required]
        public decimal BasicRate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int PositionID { get; set; }

    }
    public class EmployeeViewModel
    {
        public string Password { get; set; }
        public int EmployeeNo { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-.]?([0-9]{8})$", ErrorMessage = "Invalid Phone Numbe..!")]
        [StringLength(10)]
        public string Contact { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }
        [Required]
        public string Address { get; set; }
        //[Required]
        [Display(Name = "Position")]
        public string Description { get; set; }

        [Display(Name = "Position")]
        [Required]
        public int PositionID { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Tax Number")]
        public string Tax_No { get; set; }
    }
    public class DonationViewModel
    {
        public int DonationID { get; set; }
        public string Donor_Name { get; set; }
        public int Donor_ID { get; set; }
        public string DonorDesc { get; set; }
        public string TDescription { get; set; }
        public int DonationtypeID { get; set; }
        public string Payment_Method { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public decimal Amount { get; set; }

    }
    public class DonorViewModel
    {
        public int Donor_ID { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "Donor Type")]
        public int DonortypeID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\(?([0]{1})\)?[-. ]?([1-9]{1})[-.]?([0-9]{8})$", ErrorMessage = "Invalid Phone Numbe..!")]
        [StringLength(10)]
        public string Contact { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Tax Number")]
        public string tax_no { get; set; }
        public byte[] Image { get; set; }
    }
    public class DonationTypeViewModel
    {
        public int DonationtypeID { get; set; }
        [Required]
        public string Description { get; set; }
    }
    public class DonorTypeViewModel
    {
        public int DonortypeID { get; set; }
        [Required]
        public string Description { get; set; }
    }
    public class PayFastModel
    {
        [Key]
        public int Id { get; set; }
        public string cmd { get; set; }
        public string merchant_id { get; set; }
        public string merchant_key { get; set; }
        public string m_payment_id { get; set; }
        public string payment_mode { get; set; }
        public string name_first { get; set; }
        public string name_last { get; set; }
        public string email_address { get; set; }
        public string site { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
        public string notify_url { get; set; }
        public string item_name { get; set; }
        public decimal amount { get; set; }
        public string actionURL { get; set; }

        public PayFastModel(bool useSandbox)
        {
            this.cmd = "_donation";
            this.payment_mode = ConfigurationManager.AppSettings["PaymentMode"];
            this.merchant_id = ConfigurationManager.AppSettings["PF_MerchantID"];
            this.merchant_key = ConfigurationManager.AppSettings["PF_MerchantKey"];
            this.cancel_url = ConfigurationManager.AppSettings["PF_CancelURL"];
            this.return_url = ConfigurationManager.AppSettings["PF_ReturnURL"];

            if (useSandbox)
            {
                this.actionURL = ConfigurationManager.AppSettings["test_url"];
            }
            else
            {
                this.actionURL = ConfigurationManager.AppSettings["Prod_url"];
            }

            // We can add parameters here, for example OrderId, CustomerId, etc....
            this.notify_url = ConfigurationManager.AppSettings["notify_url"];
        }
        
    }



    public class ExpenseViewModel
    {
        public int exp_id { get; set; }

        [Display(Name = "Description")]
        [Required]
        public string desc { get; set; }

        [Display(Name = "Payed on")]
        [Required]
        public string payment_due_date { get; set; }
        [Required]

        [Display(Name = "Account Number")]
        public string exp_acc_no { get; set; }

        [Display(Name = "Account Paid From")]
        [Required]
        public string from_acc_no { get; set; }
        [Required]
        [Display(Name = "Amount")]
        [DataType(DataType.Currency)]
        public decimal amount { get; set; }

        [Display(Name = "Is this expense recurring ?")]
        public bool recurring { get; set; }

        public virtual ICollection<Expense_Paid> Expense_Paids { get; set; }
    }
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
    public class ReportViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Endate { get; set; }

        public List<Expense_PaidViewModel> Expense_PaidList { get; set; }
        public List<DonationViewModel> DonationList { get; set; }
    }
    public class Expense_PaidViewModel
    {
        public int paid_exp_id { get; set; }
        [Required]
        [Display(Name = "Expence Paid")]
        public int exp_id { get; set; }
        [Required]

        [Display(Name = "Date Paid")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_paid { get; set; }
        [Required]

        [Display(Name = "Amount Paid")]
        public decimal amount_paid { get; set; }

        [Display(Name = "Expence Paid")]
        public string ExpenseDesc { get; set; }
        [Required]
        public string Reference { get; set; }
        [Required]
        [Display(Name = "Proof of Payment")]
        public byte[] Receipt { get; set; }
        public virtual Expense Expences { get; set; }
    }
    public class SearchReport
    {

    }
}
