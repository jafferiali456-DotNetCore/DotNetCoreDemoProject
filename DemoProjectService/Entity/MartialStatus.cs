namespace DemoProjectService.Entity
{
    public class MartialStatus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int rec_status { get; set; }

        public DateTime rec_modified_on { get; set; }

        public int rec_modified_by { get; set; }

        public string? rec_action { get; set; }

        public DateTime rec_time_stamp { get; set; }

        public string? rec_rnd_key { get; set; }
    }
}
