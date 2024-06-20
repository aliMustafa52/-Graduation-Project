namespace graduationProject.Api.Contracts.Authentication
{
	public record RegisterRequest
	(
		string FirstName,
		string LastName,
		string Email,
		string Password,
		string ConfirmPassword,
		bool IsProvider,
		bool IsCustomer,
		DateOnly BirthDate,
		string Address,
		string PhoneNumber,
		string Governorate,
		//for provider
		string? Field,
		int ExperienceYears
	);
}
