namespace graduationProject.Api.Entities
{
	public class Offer
	{
		public int Id { get; set; }
		public int ProjectId { get; set; }
		public int ProviderId { get; set; }
		public string Message { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;


		public Project Project { get; set; } = default!;
		public Provider Provider { get; set; } = default!;
	}
}
