namespace graduationProject.Api.Entities
{
	public sealed class Job
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public double Price { get; set; }
		public bool IsNegotiable { get; set; }
		public DateOnly StartsAt { get; set; }
		public DateOnly EnndsAt { get; set; }
	}
}
