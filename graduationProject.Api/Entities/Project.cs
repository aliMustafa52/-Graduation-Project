namespace graduationProject.Api.Entities
{
	public class Project
	{
        public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int CreatedById { get; set; }
		public string ImageName {  get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;

		public Customer CreatedBy { get; set; } = default!;
		public ICollection<Offer> Offers { get; set; } = [];

	}
}
