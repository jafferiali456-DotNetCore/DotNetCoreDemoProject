namespace DemoProjectWebApp.Models
{
    public class Country
    {
        //public int Id { get; set; }
        //public string? CountryName { get; set; }
        //public string? CountryCode { get; set; }
        //public bool IsActive { get; set; }
        //public DateTime CreatedDate { get; set; }
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Capital { get; set; }
        public int rec_modified_by { get; set; }
        public DateTime rec_time_stamp { get; set; }
        public string? rec_rnd_key { get; set; }
        public int rec_add_by { get; set; }
        public string? Mode { get; set; }
        public string? Currency { get; set; }
    }
}
