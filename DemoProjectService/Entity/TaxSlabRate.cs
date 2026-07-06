namespace DemoProjectService.Models
{
    public class TaxSlabRate
    {
        public int TaxSlabRateID { get; set; }
        public int FiscalYearID { get; set; }
        public int Id { get; set; }
        public decimal? RangeStart { get; set; }

        public decimal? RangeEnd { get; set; }

        public decimal? ExceedingAmount { get; set; }

        public decimal? AddtionalAmountFixed { get; set; }

        public decimal? AddtionalAmountPercentage { get; set; }

        public decimal? MinimumAmountFixed { get; set; }
        public int rec_status { get; set; }
        public DateTime rec_modified_on { get; set; }
        public int rec_modified_by { get; set; }
        public DateTime rec_time_stamp { get; set; }
        public string? rec_rnd_key { get; set; }
        public int rec_add_by { get; set; }
        public string? Mode { get; set; }

    }
}