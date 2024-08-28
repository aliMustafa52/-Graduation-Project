namespace graduationProject.Api.Contracts.Customer
{
	public record CustomerResponse(int Id, string Name, string PhoneNumber, string Governorate, DateOnly BirthDate, string Address);
}
