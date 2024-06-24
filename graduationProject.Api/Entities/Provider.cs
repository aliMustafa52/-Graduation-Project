namespace graduationProject.Api.Entities
{
	public class Provider 
	{
		public int Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Governorate { get; set; } = string.Empty;
		public DateOnly BirthDate { get; set; }

		public string Field { get; set; } = string.Empty;
		public int ExperienceYears { get; set; }
		public string Address { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;
		public int CategoryId { get; set; }

		public ApplicationUser User { get; set; } = default!;
		public Category Category { get; set; } = default!;
		public ICollection<Offer> Offers { get; set; } = [];
	}
}
