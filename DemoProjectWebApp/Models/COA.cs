using System.ComponentModel.DataAnnotations;

namespace DemoProjectWebApp.Models
{
    public class COA
    {
        public decimal COAId { get; set; }

        public string? AccountCode { get; set; }

        public string? AccountDesc { get; set; }

        public string? LevelAccountNo { get; set; }

        public decimal? ParentCOAId { get; set; }

        public string? Group1 { get; set; }

        public string? Group2 { get; set; }

        public string? Group3 { get; set; }

        public string? Group4 { get; set; }

        public string? Group5 { get; set; }

        public decimal? AccountStatus { get; set; }

        public string? LinkAcc1 { get; set; }

        public string? LinkAcc2 { get; set; }

        public string? LinkAcc3 { get; set; }

        public string? LinkAcc4 { get; set; }

        public int rec_status { get; set; }

        public DateTime rec_modified_on { get; set; }

        public int rec_modified_by { get; set; }

        

        public DateTime? rec_time_stamp { get; set; }

        public string rec_rnd_key { get; set; } = string.Empty;
    }
}
