namespace graduationProject.Api.Contracts.ContactUs
{
	public record ContactUsRequest(
		 string Name,
		string Email,
		string Memo,
		string AdditionalInfo
	);
}
