namespace DemoProjectService.Entity
{
    public class GroupMaster
    {
        public int Id { get; set; }
        public int rec_status { get; set; }
        public DateTime rec_modified_on { get; set; }
        public int rec_modified_by { get; set; }
        public DateTime rec_time_stamp { get; set; }
        public string? rec_rnd_key { get; set; }
        public int rec_add_by { get; set; }
        public string? Mode { get; set; }
        public   int GroupMasterID { get; set;  }
        public   int GroupCode { get; set; }
        public   string? GroupName { get; set;  }
        public   string? Description { get; set;  }
        public List<GroupDetail> DetailList { get; set; }

    }
}