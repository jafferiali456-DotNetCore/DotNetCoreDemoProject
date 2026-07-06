using System.ComponentModel.DataAnnotations;

namespace DemoProjectService.Entity
{
    public class AddUser
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "LoginID is Required")]
        public string? LoginID { get; set; }

        // [Required(ErrorMessage = "User Name is required.")]
        public string? UserName { get; set; }

        //[Required(ErrorMessage = "Password is required.")]
        //[DataType(DataType.Password)]
        //[MinLength(14, ErrorMessage = "Password must be at least 14 characters long.")]
        public string? Password { get; set; }

        //[Required(ErrorMessage = "Confirm Password is required.")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        public string? ConfirmPassword { get; set; }

        //[Required(ErrorMessage = "Email is required.")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Designation is required.")]
        public string? Designation { get; set; }

        //[Required(ErrorMessage = "Phone number is required.")]
        //[Phone(ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }

        ///[Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        //[Required(ErrorMessage = "Department is required.")]
        //[Range(1, int.MaxValue, ErrorMessage = "Please select a valid department.")]
        public int Department { get; set; }
        public string DepartmentName { get; set; }
        public List<int> RoleRights { get; set; }
        public DateTime Rec_Modified_on { get; set; }
        public DateTime Rec_time_stamp { get; set; }
        public int Rec_add_by { get; set; }
        public int? Rec_modified_by { get; set; }
        public string? Rec_Action { get; set; }
        public int Rec_Status { get; set; }
    }
}
