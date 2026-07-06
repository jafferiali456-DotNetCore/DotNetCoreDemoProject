using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProjectService.Entity
{
    public class EmployeeOrganization
    {
        public int EmployeeOrganizationID { get; set; }

        public int EmployeeID { get; set; }
        public int Id { get; set; }


        public int DepartmentID { get; set; }

        public int DesignationID { get; set; }

        public int GradeID { get; set; }
        public int Company { get; set; }
        public int PayrollGroup { get; set; }
        public int EmpSalaryProcess { get; set; }


        public DateTime JoiningDate { get; set; }


        public string? ProbationPeriod { get; set; }

        public int BranchID { get; set; }

        public int EmployeeStatusID { get; set; }


        public DateTime HiringDate { get; set; }


        public DateTime RecordEffectiveStartDate { get; set; }


        public DateTime RecordEffectiveEndDate { get; set; }

        public int rec_status { get; set; }

        public DateTime rec_modified_on { get; set; }

        public int rec_modified_by { get; set; }


        public string? rec_action { get; set; }

        public DateTime rec_time_stamp { get; set; }


        public string? rec_rnd_key { get; set; } = "0";

        public int Group { get; set; }

        public int JobFunction { get; set; }

        public int Location { get; set; }

        public DateTime DateOfJoining { get; set; }

        public DateTime SalaryMonth { get; set; }

        public DateTime ResignationDate { get; set; }

        public string? EOBIEmployerRegCode { get; set; }

        public int BankName { get; set; }
        public string? Mode { get; set; }

        public string? BankAccountNo { get; set; }

        public string? BankCode { get; set; }

        public int BankBranch { get; set; }

        public string? IBAN { get; set; }

        public string? SwiftCode { get; set; }

        public int rec_add_by { get; set; }

        public bool FinalSettledStatus { get; set; }

        public bool SalaryProcessStatus { get; set; }

    }


}

