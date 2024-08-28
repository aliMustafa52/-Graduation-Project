namespace graduationProject.Api.Entities
{
	public sealed class Job : AuditableEntity
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string? ImageName { get; set; }	
		public DateOnly StartsAt { get; set; }
		public DateOnly EnndsAt { get; set; }
		public int ProviderId { get; set; }
		public Provider Provider { get; set; } = default!;
	}
}
