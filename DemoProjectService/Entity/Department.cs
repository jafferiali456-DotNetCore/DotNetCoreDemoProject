namespace DemoProjectService.Entity
{
    public class Department
    {
        public   int DepartmentID { get; set; }
        public   string? Code { get; set;  }
        public   string? Name { get; set;  }
        public   string? Abbreviation { get; set;  }
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