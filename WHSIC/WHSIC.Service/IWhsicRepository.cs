using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSIC.Data;

namespace WHSIC.Service
{
    public interface IWhsicRepository
    {

    }
    public interface IServiceTypeRepository : IDisposable
    {
        ServiceType GetByID(int id);
        void Save(ServiceType entity);
        List<ServiceType> getAll();
        void Update(ServiceType entity);
        void Delete(ServiceType model);
    }

    public interface IServiceeRepository : IDisposable
    {
        Servicee GetByID(int id);
        void Save(Servicee entity);
        List<Servicee> getAll();
        void Update(Servicee entity);
    }

    public interface IProjectInvoiceRepository : IDisposable
    {
        ProjectInvoice GetByID(int id);
        void Save(ProjectInvoice entity);
        List<ProjectInvoice> getAll();
        void Update(ProjectInvoice entity);
        void Delete(ProjectInvoice model);
    }

    public interface IProjectProgressRepository : IDisposable
    {
        ProjectProgress GetByID(int id);
        void Save(ProjectProgress entity);
        List<ProjectProgress> getAll();
        void Update(ProjectProgress entity);
        void Delete(ProjectProgress model);
    }

    public interface IProjectConstructorsRepository : IDisposable
    {
        ProjectConstructor GetByID(int id);
        void Save(ProjectConstructor entity);
        List<ProjectConstructor> getAll();
        void Update(ProjectConstructor entity);
        void Delete(ProjectConstructor model);
    }

    public interface IProjectRepository : IDisposable
    {
        Project GetByID(int id);
        void Save(Project entity);
        List<Project> getAll();
        void Update(Project entity);

        void Delete(Project model);
    }
    public interface INotificationRepository : IDisposable
    {
        List<Notification> getAll();
        void Save(Notification entity);
        void Update(Notification model);
    }
    public interface IVisitorRepository : IDisposable
    {
        List<VisitorsCount> getAll();
        void Save(VisitorsCount entity);
        void Update(VisitorsCount entity);
    }

    public interface IAlgorithmDatesRepository : IDisposable
    {
        AlgorithmDates GetByID(int id);
        List<AlgorithmDates> getAll();
        void Save(AlgorithmDates entity);
        void Update(AlgorithmDates entity);
    }
    public interface IAvailabilityRepository : IDisposable
    {
        Availability GetByID(int id);
        void Save(Availability entity);
        List<Availability> getAll();
        void Update(Availability entity);

        void Delete(Availability model);
    }
    public interface IGalleryRepository : IDisposable
    {
        Gallery GetByID(int id);
        void Save(Gallery entity);
        List<Gallery> getAll();
        void Delete(Gallery entity);
    }
    public interface IPaidExpenseReceiptRepository : IDisposable
    {
        PaidExpenseReceipt GetByID(int id);
        void Save(PaidExpenseReceipt entity);
        List<PaidExpenseReceipt> getAll();
        void Delete(PaidExpenseReceipt entity);
        void Update(PaidExpenseReceipt entity);
    }
    public interface ISalaryRepository : IDisposable
    {
        Salary GetByID(int id);
        void Save(Salary entity);
        List<Salary> getAll();
        void Delete(Salary entity);
        void Update(Salary entity);
    }

    public interface IMemberRepository : IDisposable
    {
        Member GetByID(string id);
        void Save(Member entity);
        List<Member> getAll();
        void Delete(Member entity);
        void Update(Member entity);
    }

    public interface IBranchRepository : IDisposable
    {
        Branch GetByID(int id);
        void Save(Branch entity);
        List<Branch> getAll();
        void Delete(Branch entity);
        void Update(Branch entity);
    }

    public interface IDonationRepository : IDisposable
    {
        Donation GetByID(int id);
        void Save(Donation entity);
        List<Donation> getAll();
        void Delete(Donation entity);
        void Update(Donation entity);
    }

    public interface IDonationTypeRepository : IDisposable
    {
        DonationType GetByID(int id);
        void Save(DonationType entity);
        List<DonationType> getAll();
        void Delete(DonationType entity);
        void Update(DonationType entity);
    }

    public interface IDonorRepository : IDisposable
    {
        Donor GetByID(int id);
        void Save(Donor entity);
        List<Donor> getAll();
        void Delete(Donor entity);
        void Update(Donor entity);
    }
    public interface IDonorTypeRepository : IDisposable
    {
        DonorType GetByID(int id);
        void Save(DonorType entity);
        List<DonorType> getAll();
        void Delete(DonorType entity);
        void Update(DonorType entity);
    }

    public interface IEmployeeRepository : IDisposable
    {
        Employee GetByID(int id);
        void Save(Employee entity);
        List<Employee> getAll();
        void Delete(Employee entity);
        void Update(Employee entity);
    }

    public interface IFullTimeRepository : IDisposable
    {
        FullTime GetByID(int id);
        void Save(FullTime entity);
        List<FullTime> getAll();
        void Delete(FullTime entity);
        void Update(FullTime entity);
    }

    public interface IPartTimeRepository : IDisposable
    {
        PartTime GetByID(int id);
        void Save(PartTime entity);
        List<PartTime> getAll();
        void Delete(PartTime entity);
        void Update(PartTime entity);
    }

    public interface IPastorRepository : IDisposable
    {
        Pastor GetByID(int id);
        void Save(Pastor entity);
        List<Pastor> getAll();
        void Delete(Pastor entity);
        void Update(Pastor entity);
    }

    public interface IPositionRepository : IDisposable
    {
        Position GetByID(int id);
        void Save(Position entity);
        List<Position> getAll();
        void Delete(Position entity);
        void Update(Position entity);
    }
    public interface IProgramRepository : IDisposable
    {
        Program GetByID(int id);
        void Save(Program entity);
        List<Program> getAll();
        void Delete(Program entity);
        void Update(Program entity);
    }

    public interface IRejectedUpload : IDisposable
    {
        Rejected GetByID(int id);
        void Save(Rejected entity);
        List<Rejected> getAll();
        void Delete(Rejected entity);
        void Update(Rejected entity);
    }

    public interface IFinalSermonRepository : IDisposable
    {
        FinalSermon GetByID(int id);
        void Save(FinalSermon entity);
        List<FinalSermon> getAll();
    }

    public interface ISermonRepository : IDisposable
    {
        Sermon GetByID(int id);
        void Save(Sermon entity);
        List<Sermon> getAll();
        void Delete(Sermon entity);
        void Update(Sermon entity);
    }

    public interface ITempUpload : IDisposable
    {
        TempUpload GetByID(int id);
        void Save(TempUpload entity);
        List<TempUpload> getAll();
        void Delete(TempUpload entity);
        void Update(TempUpload entity);
    }

    public interface IUploadRepository : IDisposable
    {
        ProgramUpload GetByID(int id);
        List<ProgramUpload> getAll();
        void Save(ProgramUpload entity);
        void Delete(ProgramUpload entity);
        void Update(ProgramUpload entity);
    }

    public interface IExpenseRepository : IDisposable
    {
        Expense GetByID(int id);
        void Save(Expense entity);
        List<Expense> getAll();
        void Delete(Expense entity);
        void Update(Expense entity);
    }
    public interface IExpense_PaidRepository : IDisposable
    {
        Expense_Paid GetByID(int id);
        void Save(Expense_Paid entity);
        List<Expense_Paid> getAll();
        void Delete(Expense_Paid entity);
        void Update(Expense_Paid entity);
    }
}
