namespace graduationProject.Api.Entities
{
	public class Customer
	{
		public int Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Governorate { get; set; } = string.Empty;
		public DateOnly BirthDate { get; set; }

		public string Address { get; set; } = string.Empty;
		public string UserId { get; set; } = string.Empty;


		public ApplicationUser User { get; set; } = default!;
	}
}
