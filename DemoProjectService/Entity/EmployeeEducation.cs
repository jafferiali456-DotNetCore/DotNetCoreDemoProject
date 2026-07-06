using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoProjectService.Entity
{
    public class EmployeeEducation
    {
        
        
            
            public int EmployeeEducationID { get; set; }
            public int Id { get; set; }

            public int EmployeeID { get; set; }


            public int rec_add_by { get; set; }


            public string? AcademicType { get; set; }
            public string? Mode { get; set; }


        public string? Institution { get; set; }

            
            public string? MajorSubject { get; set; }

            public decimal? PassingYear { get; set; }

            public string? OtherDetails { get; set; }   // varchar(max)

            public int rec_status { get; set; }

            public DateTime rec_modified_on { get; set; }

            public int rec_modified_by { get; set; }

            
            public string? rec_action { get; set; }

            public DateTime rec_time_stamp { get; set; }

            
            public string? rec_rnd_key { get; set; } = "0";
        
    }

}
