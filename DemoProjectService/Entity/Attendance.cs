namespace DemoProjectService.Entity
{
    public class Attendance
    {
        public   int AttendanceID { get; set; }
        public   int EmployeeID { get; set; }
        public   DateTime AttendanceDate { get; set;  }
        public   int ShiftMasterID { get; set; }
        public   DateTime DateTimeIn { get; set;  }
        public   DateTime DateTimeOut { get; set;  }
        public   int LeaveTypeID { get; set; }
        public   int AttendanceStatusID { get; set; }
        public   string? Description { get; set;  }
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