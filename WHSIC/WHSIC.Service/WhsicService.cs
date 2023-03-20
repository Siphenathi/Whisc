using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHSIC.Data;

namespace WHSIC.Service
{
    public class WhsicService
    {

    }

    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<ServiceType> _programies;

        public ServiceTypeRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<ServiceType>(_datacontext);
        }

        public ServiceType GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<ServiceType> getAll()
        {
            return _programies.getAll();
        }
        public void Save(ServiceType entity)
        {
            _programies.Save(entity);
        }
        public void Update(ServiceType model)
        {
            _programies.Update(model);
        }
        public void Delete(ServiceType model)
        {
            _programies.Delete(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class ServiceeRepository : IServiceeRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Servicee> _programies;

        public ServiceeRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Servicee>(_datacontext);
        }

        public Servicee GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Servicee> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Servicee entity)
        {
            _programies.Save(entity);
        }
        public void Update(Servicee model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }
    public class ProjectInvoiceRepository : IProjectInvoiceRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<ProjectInvoice> _programies;

        public ProjectInvoiceRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<ProjectInvoice>(_datacontext);
        }

        public ProjectInvoice GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<ProjectInvoice> getAll()
        {
            return _programies.getAll();
        }
        public void Save(ProjectInvoice entity)
        {
            _programies.Save(entity);
        }
        public void Update(ProjectInvoice model)
        {
            _programies.Update(model);
        }
        public void Delete(ProjectInvoice model)
        {
            _programies.Delete(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class ProjectProgressRepository : IProjectProgressRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<ProjectProgress> _programies;

        public ProjectProgressRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<ProjectProgress>(_datacontext);
        }

        public ProjectProgress GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<ProjectProgress> getAll()
        {
            return _programies.getAll();
        }
        public void Save(ProjectProgress entity)
        {
            _programies.Save(entity);
        }

        public void Update(ProjectProgress model)
        {
            _programies.Update(model);
        }

        public void Delete(ProjectProgress model)
        {
            _programies.Delete(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }
    public class ProjectConstructorsRepository : IProjectConstructorsRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<ProjectConstructor> _programies;

        public ProjectConstructorsRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<ProjectConstructor>(_datacontext);
        }

        public ProjectConstructor GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<ProjectConstructor> getAll()
        {
            return _programies.getAll();
        }
        public void Save(ProjectConstructor entity)
        {
            _programies.Save(entity);
        }

        public void Update(ProjectConstructor model)
        {
            _programies.Update(model);
        }

        public void Delete(ProjectConstructor model)
        {
            _programies.Delete(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class ProjectRepository : IProjectRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Project> _programies;

        public ProjectRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Project>(_datacontext);
        }

        public Project GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Project> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Project entity)
        {
            _programies.Save(entity);
        }

        public void Update(Project model)
        {
            _programies.Update(model);
        }

        public void Delete(Project model)
        {
            _programies.Delete(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class NotificationRepository: INotificationRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Notification> _programies;

        public NotificationRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Notification>(_datacontext);
        }

        public Notification GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Notification> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Notification entity)
        {
            _programies.Save(entity);
        }

        public void Update(Notification model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }
    public class VisitorRepository:IVisitorRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<VisitorsCount> _programies;

        public VisitorRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<VisitorsCount>(_datacontext);
        }



        public List<VisitorsCount> getAll()
        {
            return _programies.getAll();
        }
        public void Save(VisitorsCount entity)
        {
            _programies.Save(entity);
        }

        public void Update(VisitorsCount model)
        {
            _programies.Update(model);
        }

        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }
    public class AlgorithmDateRepository:IAlgorithmDatesRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<AlgorithmDates> _programies;

        public AlgorithmDateRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<AlgorithmDates>(_datacontext);
        }


        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }

        public AlgorithmDates GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<AlgorithmDates> getAll()
        {
            return _programies.getAll();
        }

        public void Save(AlgorithmDates entity)
        {
            _programies.Save(entity);
        }

        public void Update(AlgorithmDates model)
        {
            _programies.Update(model);
        }
    }
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Availability> _programies;

        public AvailabilityRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Availability>(_datacontext);
        }

        public List<Availability> getAll()
        {
            return _programies.getAll();
        }
        public Availability GetByID(int id)
        {
            return _programies.GetById(id);
        }
        public void Save(Availability model)
        {
            _programies.Save(model);
        }

        public void Update(Availability model)
        {
            _programies.Update(model);
        }

        public void Delete(Availability model)
        {
            _programies.Delete(model);
        }

        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }

    }
    public class GalleryRepository:IGalleryRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Gallery> _programies;

        public GalleryRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Gallery>(_datacontext);
        }

        public List<Gallery> getAll()
        {
            return _programies.getAll();
        }
        public Gallery GetByID(int id)
        {
            return _programies.GetById(id);
        }
        public void Save(Gallery model)
        {
            _programies.Save(model);
        }
        public void Delete(Gallery entity)
        {
            _programies.Delete(entity);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }


    }

    public class PaidExpenseReceiptRepository: IPaidExpenseReceiptRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<PaidExpenseReceipt> _programies;

        public PaidExpenseReceiptRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<PaidExpenseReceipt>(_datacontext);
        }

        public PaidExpenseReceipt GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<PaidExpenseReceipt> getAll()
        {
            return _programies.getAll();
        }
        public void Save(PaidExpenseReceipt model)
        {
            _programies.Save(model);
        }

        public void Delete(PaidExpenseReceipt entity)
        {
            _programies.Delete(entity);
        }

        public void Update(PaidExpenseReceipt model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }
    public class SalaryRepository : ISalaryRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Salary> _programies;

        public SalaryRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Salary>(_datacontext);
        }

        public Salary GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Salary> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Salary model)
        {
            _programies.Save(model);
        }

        public void Delete(Salary entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Salary model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }



    public class MemberRepository : IMemberRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Member> _programies;

        public MemberRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Member>(_datacontext);
        }

        public Member GetByID(string  id)
        {
            return _programies.GetById(id);
        }

        public List<Member> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Member model)
        {
            _programies.Save(model);
        }

        public void Delete(Member entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Member model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }


    public class BranchRepository : IBranchRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Branch> _programies;

        public BranchRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Branch>(_datacontext);
        }

        public Branch GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Branch> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Branch model)
        {
            _programies.Save(model);
        }

        public void Delete(Branch entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Branch model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class DonationRepository : IDonationRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Donation> _programies;

        public DonationRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Donation>(_datacontext);
        }

        public Donation GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Donation> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Donation model)
        {
            _programies.Save(model);
        }

        public void Delete(Donation entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Donation model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class DonationTypeRepository : IDonationTypeRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<DonationType> _programies;

        public DonationTypeRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<DonationType>(_datacontext);
        }

        public DonationType GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<DonationType> getAll()
        {
            return _programies.getAll();
        }
        public void Save(DonationType model)
        {
            _programies.Save(model);
        }

        public void Delete(DonationType entity)
        {
            _programies.Delete(entity);
        }

        public void Update(DonationType model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class DonorRepository : IDonorRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Donor> _programies;

        public DonorRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Donor>(_datacontext);
        }

        public Donor GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Donor> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Donor model)
        {
            _programies.Save(model);
        }

        public void Delete(Donor entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Donor model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class DonorTypeRepository:IDonorTypeRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<DonorType> _programies;

        public DonorTypeRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<DonorType>(_datacontext);
        }

        public DonorType GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<DonorType> getAll()
        {
            return _programies.getAll();
        }
        public void Save(DonorType model)
        {
            _programies.Save(model);
        }

        public void Delete(DonorType entity)
        {
            _programies.Delete(entity);
        }

        public void Update(DonorType model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class EmployeeRepository : IEmployeeRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Employee> _programies;

        public EmployeeRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Employee>(_datacontext);
        }

        public Employee GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Employee> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Employee model)
        {
            _programies.Save(model);
        }

        public void Delete(Employee entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Employee model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class FullTimeRepository : IFullTimeRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<FullTime> _programies;

        public FullTimeRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<FullTime>(_datacontext);
        }

        public FullTime GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<FullTime> getAll()
        {
            return _programies.getAll();
        }
        public void Save(FullTime model)
        {
            _programies.Save(model);
        }

        public void Delete(FullTime entity)
        {
            _programies.Delete(entity);
        }

        public void Update(FullTime model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }

    }
    public class PartTimeRepository : IPartTimeRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<PartTime> _programies;

        public PartTimeRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<PartTime>(_datacontext);
        }

        public PartTime GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<PartTime> getAll()
        {
            return _programies.getAll();
        }
        public void Save(PartTime model)
        {
            _programies.Save(model);
        }

        public void Delete(PartTime entity)
        {
            _programies.Delete(entity);
        }

        public void Update(PartTime model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }

    }

    public class PastorRepository : IPastorRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Pastor> _programies;

        public PastorRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Pastor>(_datacontext);
        }

        public Pastor GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Pastor> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Pastor model)
        {
            _programies.Save(model);
        }

        public void Delete(Pastor entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Pastor model)
        {
            _programies.Update(model);


        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }


    public class PositionRepository : IPositionRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Position> _programies;

        public PositionRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Position>(_datacontext);
        }

        public Position GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Position> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Position model)
        {
            _programies.Save(model);
        }

        public void Delete(Position entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Position model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }

    }

    public class ProgramRepository : IProgramRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Program> _programies;

        public ProgramRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Program>(_datacontext);
        }

        public Program GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Program> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Program model)
        {
            _programies.Save(model);
        }

        public void Delete(Program entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Program model)
        {
            _programies.Update(model);


        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

    public class RejectedUploadsRepository : IRejectedUpload
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Rejected> _uploads;

        public RejectedUploadsRepository()
        {
            _datacontext = new ApplicationDbContext();
            _uploads = new RepositoryService<Rejected>(_datacontext);
        }

        public Rejected GetByID(int id)
        {
            return _uploads.GetById(id);
        }

        public List<Rejected> getAll()
        {
            return _uploads.getAll();
        }
        public void Save(Rejected model)
        {
            _uploads.Save(model);
        }

        public void Delete(Rejected entity)
        {
            _uploads.Delete(entity);
        }

        public void Update(Rejected model)
        {
            _uploads.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }


    public class FinalSermonRepository : IFinalSermonRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<FinalSermon> _programies;

        public FinalSermonRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<FinalSermon>(_datacontext);
        }

        public FinalSermon GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<FinalSermon> getAll()
        {
            return _programies.getAll();
        }
        public void Save(FinalSermon model)
        {
            _programies.Save(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }


    public class SermonRepository : ISermonRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Sermon> _programies;

        public SermonRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Sermon>(_datacontext);
        }

        public Sermon GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Sermon> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Sermon model)
        {
            _programies.Save(model);
        }

        public void Delete(Sermon entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Sermon model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }


    public class TempUploadRepository : ITempUpload
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<TempUpload> _tuploads;

        public TempUploadRepository()
        {
            _datacontext = new ApplicationDbContext();
            _tuploads = new RepositoryService<TempUpload>(_datacontext);
        }

        public TempUpload GetByID(int id)
        {
            return _tuploads.GetById(id);
        }

        public List<TempUpload> getAll()
        {
            return _tuploads.getAll();
        }
        public void Save(TempUpload model)
        {
            _tuploads.Save(model);
        }

        public void Delete(TempUpload entity)
        {
            _tuploads.Delete(entity);
        }

        public void Update(TempUpload model)
        {
            _tuploads.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }

    }

    public class UploadRepository : IUploadRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<ProgramUpload> _uploads;

        public UploadRepository()
        {
            _datacontext = new ApplicationDbContext();
            _uploads = new RepositoryService<ProgramUpload>(_datacontext);
        }

        public ProgramUpload GetByID(int id)
        {
            return _uploads.GetById(id);
        }

        public List<ProgramUpload> getAll()
        {
            return _uploads.getAll();
        }

        public void Save(ProgramUpload model)
        {
            _uploads.Save(model);
        }
        public void Delete(ProgramUpload entity)
        {
            _uploads.Delete(entity);
        }

        public void Update(ProgramUpload model)
        {
            _uploads.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }
    }

   public class ExpenseRepository:IExpenseRepository
   {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Expense> _programies;

    public ExpenseRepository()
    {
        _datacontext = new ApplicationDbContext();
        _programies = new RepositoryService<Expense>(_datacontext);
    }

    public Expense GetByID(int id)
    {
        return _programies.GetById(id);
    }

    public List<Expense> getAll()
    {
        return _programies.getAll();
    }
    public void Save(Expense model)
    {
        _programies.Save(model);
    }

    public void Delete(Expense entity)
    {
        _programies.Delete(entity);
    }

    public void Update(Expense model)
    {
        _programies.Update(model);
    }
    public void Dispose()
    {
        _datacontext.Dispose();
        _datacontext = null;
    }

   }


    public class Expense_PaidRepository: IExpense_PaidRepository
    {
        private ApplicationDbContext _datacontext = null;
        private readonly IRepository<Expense_Paid> _programies;

        public Expense_PaidRepository()
        {
            _datacontext = new ApplicationDbContext();
            _programies = new RepositoryService<Expense_Paid>(_datacontext);
        }

        public Expense_Paid GetByID(int id)
        {
            return _programies.GetById(id);
        }

        public List<Expense_Paid> getAll()
        {
            return _programies.getAll();
        }
        public void Save(Expense_Paid model)
        {
            _programies.Save(model);
        }

        public void Delete(Expense_Paid entity)
        {
            _programies.Delete(entity);
        }

        public void Update(Expense_Paid model)
        {
            _programies.Update(model);
        }
        public void Dispose()
        {
            _datacontext.Dispose();
            _datacontext = null;
        }

    }

}
