

namespace DemoProjectService.Entity
{
    public class DownTimeclass
    {
        public int Id { get; set; }
        
        public string? Reason { get; set; }
        public string? Description { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime ShowDateTime { get; set; }
        public DateTime EndDate { get; set; }
        public int RecAddBy { get; set; }
        public int RecModifiedBy { get; set; }
        public DateTime RecTimeStamp { get; set; }
        public DateTime RecMoifiedOn { get; set; }

    }
}
