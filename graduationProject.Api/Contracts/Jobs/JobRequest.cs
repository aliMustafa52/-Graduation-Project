namespace graduationProject.Api.Contracts.Jobs
{
	public record JobRequest(
		string Title,
		string Description,
		DateOnly StartsAt,
		DateOnly EnndsAt,
		IFormFile? ImageFile
	);

}
