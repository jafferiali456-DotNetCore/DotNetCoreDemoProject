using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProjectService.Entity
{
    public class GoalSetting
    {
        public int GoalObjectiveID { get; set; }
        public int GoalSettingID { get; set; }
        public int EmployeeID { get; set; }
        public int ComRowID { get; set; }
        public int DepartmentID { get; set; }
        public int FiscalYearID { get; set; }
        public int LineManagerID { get; set; }
        public int CurrentStep { get; set; }
        public int FinalRating { get; set; }
        public int SubmittedBy { get; set; }
        public int ApprovedBy { get; set; }
        public int RejectedBy { get; set; }
        public string? AppraisalType { get; set; }
        public string? RejectionReason { get; set; }
        public string? Designation { get; set; }
        public string? Unit { get; set; }
        public string? Status { get; set; }
        public string? LineManager { get; set; }
        public string? OverallComment { get; set; }
        public string? HODComment { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime SubmittedOn { get; set; }
        public DateTime PeriodTo { get; set; }
        public DateTime DateOfJoining { get; set; }
        public DateTime DateOfConfirmation { get; set; }
        public DateTime ApprovedOn { get; set; }
        public DateTime RejectedOn { get; set; }

        public int Id { get; set; }
        public int rec_status { get; set; }
        public DateTime rec_modified_on { get; set; }
        public int rec_modified_by { get; set; }
        public DateTime rec_time_stamp { get; set; }
        public string? rec_rnd_key { get; set; }
        public int rec_add_by { get; set; }
        public string? Mode { get; set; }
        

    }
}