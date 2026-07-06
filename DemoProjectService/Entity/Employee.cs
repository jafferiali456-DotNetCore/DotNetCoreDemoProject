namespace DemoProjectService.Entity
{
    public class Employee
    {
        
            public int EmployeeID { get; set; }
            public int Id { get; set; }

            public string? EmployeeCode { get; set; }

            public string? Name { get; set; }

            public string? FatherName { get; set; }

            public string? ShortName { get; set; }

            public bool FinalSettledStatus { get; set; }

            public bool SalaryProcessStatus { get; set; }

            public int EmpSalaryProcessGroupID { get; set; }

            public string? Gender { get; set; }

            public string? CNIC { get; set; }

            public string? Passport { get; set; }

            public string? PermenantAddress { get; set; }

            public string? JobDesc { get; set; }

            public int JobFuncID { get; set; }

            public int PermenantCityID { get; set; }

            public string? TemporaryAddress { get; set; }

            public int TemporaryCityID { get; set; }

            public DateTime DateOfBirth { get; set; }

            public int DomicileID { get; set; }

            public int ReligionID { get; set; }

            public int SectsID { get; set; }

            public string? DrivingLicenceNo { get; set; }

            public int NationalityID { get; set; }

            public int MartialStatusID { get; set; }

            public string? NextOfKin { get; set; }

            public string? EOBIEmployerSubRegNo { get; set; }

            public string? SpokenLanguages { get; set; }

            public string? IllnessPrecaution { get; set; }

            public string? OtherRemarks { get; set; }

            public DateTime JoiningDate { get; set; }

            public DateTime SalaryMonth { get; set; }

            public int DepartmentID { get; set; }

            public int DesignationID { get; set; }

            public int GradeId { get; set; }

            public int LocationID { get; set; }

            public int CategoryID { get; set; }

            public decimal? Salary { get; set; }

            public int GroupID { get; set; }

            public string? BankName { get; set; }

            public string? BankBranch { get; set; }

            public string? BankAccountNo { get; set; }

            public string? ProbationPeriod { get; set; }

            public int BranchID { get; set; }

            public int EmployeeStatusID { get; set; }

            public string? Manager { get; set; }

            public DateTime HiringDate { get; set; }

            public DateTime RecordEffectiveStartDate { get; set; }

            public DateTime RecordEffectiveEndDate { get; set; }

            public string? FinancialChange { get; set; }

            public DateTime ResignationDate { get; set; }

            public string? EmpPassword { get; set; }

            public decimal? OverTimeRate { get; set; }

            public int BankID { get; set; }

            public int CompanyID { get; set; }

            public string? NTN { get; set; }

            public int BankWithdrawalID { get; set; }

            public bool Taxable { get; set; }

            public string? LocationCode { get; set; }

            public string? SwiftCode { get; set; }

            public string? IBAN { get; set; }

            public string? BankingCode { get; set; }

            public string? BranchCode { get; set; }

            public int rec_status { get; set; }

            public DateTime rec_modified_on { get; set; }

            public int rec_modified_by { get; set; }

            public string? rec_action { get; set; }

            public DateTime rec_time_stamp { get; set; }

            public string? rec_rnd_key { get; set; }

            public bool GratuityStatus { get; set; }

            public int ParentEmployeeID { get; set; }

            public List<EmployeeSalary> employeeSalaries { get; set; }  
            public List<EmployeeOrganization> employeeOrganizations { get; set; }  
            public List<EmployeeEducation> employeeEducations { get; set; }  
            public List<ContactType> contactTypes { get; set; }  
            public List<EmployeeFamily> employeeFamilies { get; set; }  
        

    }
}
