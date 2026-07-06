namespace DemoProjectService.Entity
{
    public class EmpGroupProcess
    {
        public int Id { get; set; }
        public decimal EmpGroupProcessID { get; set; }

        public decimal? ProcessCode { get; set; }

        public string? ProcessName { get; set; }
        public string? Mode { get; set; }

        public string? ProcessBehavior { get; set; }

        public string? D_C { get; set; }

        public string? SubLedgerCode { get; set; }

        public int rec_status { get; set; }
        public int rec_add_by { get; set; }

        public DateTime? rec_modified_on { get; set; }

        public int rec_modified_by { get; set; }

        public string? rec_action { get; set; }

        public DateTime? rec_time_stamp { get; set; }

        public string? rec_rnd_key { get; set; }
    }
}