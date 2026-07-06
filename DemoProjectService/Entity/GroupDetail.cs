namespace DemoProjectService.Entity
{
    public class GroupDetail
    {
        public int Id { get; set; }
        public int rec_status { get; set; }
        public DateTime rec_modified_on { get; set; }
        public int rec_modified_by { get; set; }
        public DateTime rec_time_stamp { get; set; }
        public string? rec_rnd_key { get; set; }
        public int rec_add_by { get; set; }
        public string? Mode { get; set; }
        public   int GroupDetailID { get; set;  }
        public   int GroupMasterID { get; set;  }
        public   string? AccountCode { get; set;  }
        public   string? AccountType { get; set;  }
        public   int GrpProcessCode { get; set; }
        public   string? SourceBaseValue { get; set;  }
        public   string? LedgerCode { get; set;  }
        public   string? CalcType { get; set;  }
        public string? Prorate { get; set; }
        public   string? Duration { get; set;  }
        public   string? Description { get; set;  }
        public  bool Taxable { get; set; }
        public   string? Computation { get; set;  }
        public   string? CalcImpact { get; set;  }

    }
}