namespace DemoProjectService.Entity
{
    public class KPI
    {
        public   int KPIID { get; set; }
        public   int GoalObjectiveID { get; set; }
        public   string? KPIDescription { get; set;  }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Weightage { get; set; }  
        public decimal Achievement { get; set; }  
        public decimal SelfRating { get; set; }  
        public decimal ManagerRating { get; set; }  

        public   string? TargetValue { get; set;  }
        public   string? ActualValue { get; set;  }
        public   string? Comments { get; set;  }
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