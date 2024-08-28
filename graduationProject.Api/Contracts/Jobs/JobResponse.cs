namespace graduationProject.Api.Contracts.Jobs
{
	public record JobResponse(
		int Id,
		string Title,
		string Description,
		DateOnly StartsAt,
		DateOnly EnndsAt,
		string ImageName
	);
}
