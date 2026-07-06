namespace DemoProjectWebApp.Models
{
	public class ValueSetMaster
	{
		public int Id { get; set; }
		public decimal ValueSetID { get; set; }
		public string? ValueSetMasterCode { get; set; }
		public string? ValueSetName { get; set; }
		public string? ValueSetType { get; set; }
		public int? VMLevels { get; set; }
		public int? rec_status { get; set; }
		public DateTime? rec_modified_on { get; set; }
		public decimal? rec_modified_by { get; set; }
		public string? rec_action { get; set; }
		public DateTime? rec_time_stamp { get; set; }
		public string? rec_rnd_key { get; set; }
	}
}
