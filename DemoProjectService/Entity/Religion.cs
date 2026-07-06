using System.ComponentModel.DataAnnotations;

namespace PayrollWebApp.Models
{
	public class Religion
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Name is Requried")]
		public string? Name { get; set; }
		public string? Mode { get; set; }

		[Required(ErrorMessage = "Code is Requried")]
		public string? Code { get; set; }

		[Required(ErrorMessage = "Description is Requried")]
		public string? Description { get; set; }

		public int rec_status { get; set; }
		public string? rec_action { get; set; }
		public DateTime rec_modified_on { get; set; }
		public DateTime rec_time_stamp { get; set; }
		public int rec_modified_by { get; set; }
		public int rec_add_by { get; set; }
		public string? rec_rnd_key { get; set; }

	}
}
