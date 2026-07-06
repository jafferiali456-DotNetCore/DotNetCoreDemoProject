using System.ComponentModel.DataAnnotations;

namespace DemoProjectWebApp.Models
{
    public class PopUpUserModel
    {
        public int Id { get;set; }
        [Required(ErrorMessage = "Required Account Type")]
        public string? AccountType { get; set; }
        [Required(ErrorMessage = "Required Nature Of Account")]
        public string? NatureOfAccount { get; set; }
        [Required(ErrorMessage = "Required Cif No")]
        public string? CifNo { get; set; }
        public string? JoinTyp { get; set; }
    }
}
