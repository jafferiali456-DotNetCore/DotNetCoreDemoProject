namespace DemoProjectWebApp.Models
{
    public class RoleRights
    {
        public int Id { get; set; }        
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public DateTime Rec_Modified_on { get; set; }
        public DateTime Rec_time_stamp { get; set; }
        public int Rec_add_by { get; set; }
        public int? Rec_modified_by { get; set; }
        public string? Rec_Action { get; set; }
        public int Rec_Status { get; set; }

        public List<Rights> userRight { get; set; }
        
    }
}
