namespace DemoProjectService.Entity
{
    public class ContactType
    {
        
            public int EmployeeID { get; set; }
            public int ContactTypeId { get; set; }
            public int Id { get; set; }
            public string? Mode { get; set; }
            public int rec_add_by { get; set; }
            public string? EmployeeCode { get; set; }

            public string? Name { get; set; }            
            public int rec_status { get; set; }

            public DateTime rec_modified_on { get; set; }

            public int rec_modified_by { get; set; }

            public string? rec_action { get; set; }

            public DateTime rec_time_stamp { get; set; }

            public string? rec_rnd_key { get; set; }


        

    }
}
