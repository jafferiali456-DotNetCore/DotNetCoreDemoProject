using MicroserviesWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Cmp;
using DemoProjectService.Controllers;
using DemoProjectService.Entity;
using DemoProjectService.Models;
using PayrollWebApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MicroserviesWebApplication.Generic_Repo
{

	[ApiController]
	[Route("api/RoleRights")]

	public class RoleRightsController : GenericController<RoleRights>
	{

		public RoleRightsController(IRepository<RoleRights> repository) : base(repository) { }
	}


	[ApiController]
	[Route("api/AddUser")]

	public class AddUserController : GenericController<AddUser>
	{

		public AddUserController(IRepository<AddUser> repository) : base(repository) { }
	}

	[ApiController]
	[Route("api/DepartmentDropDown")]

	public class DepertmentController : GenericController<WF_Department>
	{
		//  public IConfiguration _configuration { get; }
		public DepertmentController(IRepository<WF_Department> repository) : base(repository) { }
	}

    [ApiController]
    [Route("api/DownTimeclass")]
    public class DownTimeclassController : GenericController<DownTimeclass>
    {
        public DownTimeclassController(IRepository<DownTimeclass> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/Country")]
    public class CountryController : GenericController<Country>
    {
        public CountryController(IRepository<Country> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/City")]
    public class CityController : GenericController<City>
    {
        public CityController(IRepository<City> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/DynamicField")]
    public class DynamicFieldController : GenericController<DynamicField>
    {
        public DynamicFieldController(IRepository<DynamicField> repository) : base(repository) { }
    }
    
    [ApiController]
    [Route("api/PayrollForm")]
    public class PayrollFormController : GenericController<PayrollForm>
    {
        public PayrollFormController(IRepository<PayrollForm> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/ValueSetMaster")]
    public class ValuSetMasterController : GenericController<ValueSetMaster>
    {
        public ValuSetMasterController(IRepository<ValueSetMaster> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/ValueSetDetail")]
    public class ValuSetDetailController : GenericController<ValueSetDetail>
    {
        public ValuSetDetailController(IRepository<ValueSetDetail> repository) : base(repository) { }
    }
    [ApiController]
    [Route("api/Company")]
    public class CompanyController : GenericController<Company>
    {
        public CompanyController(IRepository<Company> repository) : base(repository) { }
    }


    
    [ApiController]
    [Route("api/Religion")]
    public class ReligionController : GenericController<Religion>
    {
        public ReligionController(IRepository<Religion> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/Designation")]
    public class DesignationController : GenericController<Designation>
    {
        public DesignationController(IRepository<Designation> repository) : base(repository) { }
    }





    [ApiController]
    [Route("api/FiscalYear")]
    public class FiscalYearController : GenericController<FiscalYear>
    {
        public FiscalYearController(IRepository<FiscalYear> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/Bank")]
    public class BankController : GenericController<Bank>
    {
        public BankController(IRepository<Bank> repository) : base(repository) { }
    }





    [ApiController]
    [Route("api/Department")]
    public class DepartmentController : GenericController<Department>
    {
        public DepartmentController(IRepository<Department> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/EmployeeStatus")]
    public class EmployeeStatusController : GenericController<EmployeeStatus>
    {
        public EmployeeStatusController(IRepository<EmployeeStatus> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/Grade")]
    public class GradeController : GenericController<Grade>
    {
        public GradeController(IRepository<Grade> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/TaxSlabRate")]
    public class TaxSlabRateController : GenericController<TaxSlabRate>
    {
        public TaxSlabRateController(IRepository<TaxSlabRate> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/Attendance")]
    public class AttendanceController : GenericController<Attendance>
    {
        public AttendanceController(IRepository<Attendance> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/JobFunc")]
    public class JobFuncController : GenericController<JobFunc>
    {
        public JobFuncController(IRepository<JobFunc> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/Location")]
    public class LocationController : GenericController<Location>
    {
        public LocationController(IRepository<Location> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/ReasonType")]
    public class ReasonTypeController : GenericController<ReasonType>
    {
        public ReasonTypeController(IRepository<ReasonType> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/Shift")]
    public class ShiftController : GenericController<Shift>
    {
        public ShiftController(IRepository<Shift> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/LeaveType")]
    public class LeaveTypeController : GenericController<LeaveType>
    {
        public LeaveTypeController(IRepository<LeaveType> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/EmployeeMaster")]
    public class EmployeeMasterController : GenericController<EmployeeMaster>
    {
        public EmployeeMasterController(IRepository<EmployeeMaster> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/Employee")]
    public class EmployeeController : GenericController<Employee>
    {
        public EmployeeController(IRepository<Employee> repository) : base(repository) { }
    }
    
    [ApiController]
    [Route("api/Domicile")]
    public class DomicileController : GenericController<Domicile>
    {
        public DomicileController(IRepository<Domicile> repository) : base(repository) { }
    }
    [ApiController]
    [Route("api/MartialStatus")]
    public class MartialStatusController : GenericController<MartialStatus>
    {
        public MartialStatusController(IRepository<MartialStatus> repository) : base(repository) { }
    }
    [ApiController]
    [Route("api/Sects")]
    public class SectsController : GenericController<Sects>
    {
        public SectsController(IRepository<Sects> repository) : base(repository) { }
    }
    [ApiController]
    [Route("api/Nationality")]
    public class NationalityController : GenericController<Nationality>
    {
        public NationalityController(IRepository<Nationality> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/ContactType")]
    public class ContactTypeController : GenericController<ContactType>
    {
        public ContactTypeController(IRepository<ContactType> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/EmployeeSalary")]
    public class EmployeeSalaryController : GenericController<EmployeeSalary>
    {
        public EmployeeSalaryController(IRepository<EmployeeSalary> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/EmployeeOrganization")]
    public class EmployeeOrganizationController : GenericController<EmployeeOrganization>
    {
        public EmployeeOrganizationController(IRepository<EmployeeOrganization> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/EmployeeFamily")]
    public class EmployeeFamilyController : GenericController<EmployeeFamily>
    {
        public EmployeeFamilyController(IRepository<EmployeeFamily> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/EmployeeEducation")]
    public class EmployeeEducationController : GenericController<EmployeeEducation>
    {
        public EmployeeEducationController(IRepository<EmployeeEducation> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/GroupMaster")]
    public class GroupMasterController : GenericController<GroupMaster>
    {
        public GroupMasterController(IRepository<GroupMaster> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/GroupDetail")]
    public class GroupDetailController : GenericController<GroupDetail>
    {
        public GroupDetailController(IRepository<GroupDetail> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/COA")]
    public class COAController : GenericController<COA>
    {
        public COAController(IRepository<COA> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/ScheduleTask")]
    public class ScheduleTaskController : GenericController<ScheduleTask>
    {
        public ScheduleTaskController(IRepository<ScheduleTask> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/Grouping")]
    public class GroupingController : GenericController<Grouping>
    {
        public GroupingController(IRepository<Grouping> repository) : base(repository) { }
    }


    [ApiController]
    [Route("api/EmpGroupProcess")]
    public class EmpGroupProcessController : GenericController<EmpGroupProcess>
    {
        public EmpGroupProcessController(IRepository<EmpGroupProcess> repository) : base(repository) { }
    }

    [ApiController]
    [Route("api/GoalObjective")]
    public class GoalObjectiveController : GenericController<GoalObjective>
    {
        public GoalObjectiveController(IRepository<GoalObjective> repository) : base(repository) { }
    }
}
