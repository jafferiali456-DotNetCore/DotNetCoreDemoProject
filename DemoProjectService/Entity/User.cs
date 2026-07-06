

namespace DemoProjectService.Entity
{ 
    public class User
    {
        public long Id { get; set; }
        public long DepartmentID
        {
            get; set;
        }
        public long UserID
        {
            get; set;
        }
        public string LoginID
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Password
        {
            get; set;
        }
        public string EmployeeCode
        {
            get; set;
        }
        public long Designation
        {
            get; set;
        }
        public long ReportTo
        {
            get; set;
        }
        public string Phone
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public int rec_status { get; set; }
        public DateTime rec_modified_on { get; set; }
        public long rec_modified_by {get;set;}
	public string rec_action { get; set; }
        public DateTime rec_time_stamp { get; set; }
        
	public string rec_rnd_key { get; set; }
        public string SaltKey { get; set; }
	public long LocationID { get; set; }
public string UserType { get; set; }
public bool IsBasicAuth { get; set; }
	public string UserCode { get; set; }

        //public ArrayList RoleRights
        //{
        //    get { return _alRoleRights; }
        //    set { _alRoleRights = value; }
        //}

            
       
    }
}
