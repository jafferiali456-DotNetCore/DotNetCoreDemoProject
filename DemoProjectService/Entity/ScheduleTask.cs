namespace DemoProjectService.Models
{
    public class ScheduleTask
    {
        public   int SchTaskID { get; set; }
        public   string? TransactionType { get; set;  }
        public string? EmployeeID { get; set; }
        public string? AccountCode { get; set; }
        public string? Action { get; set; }
        public string? GrpProcessCode { get; set; }
        public   DateTime PeriodStartDate { get; set;  }
        public   DateTime PeriodEndDate { get; set;  }
        public   int PRGID { get; set; }
        public   string? Description { get; set;  }
        public   decimal? OriginalAmount { get; set;  }
        public   decimal? RateCalc { get; set;  }
        public   bool Taxable { get; set;  }
        public   int FiscalYearID { get; set; }
        public   int LoanMasterID { get; set; }
        public   string? LocationCode { get; set;  }
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