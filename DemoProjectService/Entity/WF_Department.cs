namespace DemoProjectService.Models
{
    public class WF_Department
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Rec_add_by { get; set; }
        public int Rec_modified_by { get; set; }
        public int Rec_Status { get; set; }
        public string? Rec_Action { get; set; }
        public DateTime Rec_Modified_on { get; set; }
        public DateTime Rec_time_stamp { get; set; }
    }
}
