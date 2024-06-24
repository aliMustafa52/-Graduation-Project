namespace graduationProject.Api.Contracts.ContactUs
{
	public record ContactUsResponse(
		 int Id,
		string Name,
		string Email,
		string Memo,
		string AdditionalInfo
		);
}
