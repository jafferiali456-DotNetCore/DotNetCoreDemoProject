using System.ComponentModel.DataAnnotations;

namespace DemoProjectWebApp.Models
{
    public class DownTimeclass
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Reason required.")]
        public string? Reason { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Start Date required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Show DateTime required.")]
        public DateTime ShowDateTime { get; set; }
        [Required(ErrorMessage = "End Date required.")]
        public DateTime EndDate { get; set; }   
        public int RecAddBy { get; set; }
        public int RecModifiedBy { get; set; }
        public DateTime RecTimeStamp { get; set; }  
        public DateTime RecMoifiedOn { get; set; }

    }
}
