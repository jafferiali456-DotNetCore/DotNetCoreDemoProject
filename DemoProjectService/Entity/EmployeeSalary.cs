using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProjectService.Entity
{
    public class EmployeeSalary
    {

        public decimal EmployeeSalaryID { get; set; }

        public int EmployeeID { get; set; }
        public int Id { get; set; }

        public string? ResponseMessage { get; set; }
        public bool IsSuccess { get; set; }
        public decimal? BasicSalary { get; set; }

        public bool TempSalaryStatus { get; set; }


        public DateTime RecordEffectiveDateStart { get; set; }


        public DateTime RecordEffectiveDateEnd { get; set; }

        public int DepartmentID { get; set; }

        public int DesignationID { get; set; }

        public int GradeId { get; set; }

        public int LocationID { get; set; }


        public string? CostCentreCode { get; set; }


        public string? CompanyBranch { get; set; }

        public int rec_status { get; set; }

        public DateTime rec_modified_on { get; set; }

        public int rec_modified_by { get; set; }
        public int rec_add_by { get; set; }


        public string? rec_action { get; set; }
        public string? Mode { get; set; }

        public DateTime rec_time_stamp { get; set; }


        public string? rec_rnd_key { get; set; }


        public DateTime TempDate { get; set; }


    }
}
