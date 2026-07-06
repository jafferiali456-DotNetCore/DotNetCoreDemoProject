namespace DemoProjectService.Entity
{
    public class Country
    {
        public int Id { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
