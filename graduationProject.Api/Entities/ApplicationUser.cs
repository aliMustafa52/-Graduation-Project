using System.Numerics;

namespace graduationProject.Api.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public DateOnly BirthDate { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime TimeAddUser { get; set; }
		public string? ProfileImage { get; set; }


		public List<RefreshToken> RefreshTokens { get; set; } = [];
		public ICollection<Provider> Providers { get; set; } = [];
		public ICollection<Customer> Customers { get; set; } = [];

	}
}
