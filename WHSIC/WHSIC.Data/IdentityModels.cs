using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace WHSIC.Data
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProgramUpload> ProgramUploads { get; set; }
        public DbSet<TempUpload> TempUploads { get; set; }
        public DbSet<Rejected> Rejecteds { get; set; }
        public DbSet<AppointmentCalendar> Appointments { get; set; }
        public DbSet<Pastor> Pastors { get; set; }
        public DbSet<Sermon> Sermons { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FullTime> FullTimes { get; set; }
        public DbSet<PartTime> PartTimes { get; set; }
        public DbSet<DonationType> DonationType { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonorType> DonorType { get; set; }
        public DbSet<Donor> Donor { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Expense_Paid> Expense_Paids { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<PaidExpenseReceipt> PaidExpenseReceipts { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<StoreCount> StoreCounts { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<AlgorithmDates> AlgorithmDateses { get; set; }
        public DbSet<VisitorsCount> VisitorsCounts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectConstructor> ProjectConstructors { get; set; }
        public DbSet<ProjectProgress> ProjectProgresses { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Servicee> Servicees { get; set; }
        public DbSet<Event>Events { get; set; }
        public DbSet<EventGuest> EventGuests { get; set; }
        public DbSet<Asset_Category> Asset_Categories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetCharge> AssetCharges { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Damaged_Asset> Damaged_Assets { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<Asset_Renter> Asset_Renters { get; set; }
        public DbSet <Asset_Rentalz> Asset_Rentals { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Subscibers> Subscibers { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


    }

    public class Income
    {
        [Key]
        public int IncomeID { get; set; }
        public decimal TotIncome { get; set; }
    }
}