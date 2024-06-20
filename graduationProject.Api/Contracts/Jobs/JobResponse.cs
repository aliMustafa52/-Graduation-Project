namespace graduationProject.Api.Contracts.Jobs
{
	public record JobResponse(
		int Id,
		string Title,
		string Description,
		double Price,
		bool IsNegotiable,
		DateOnly StartsAt,
		DateOnly EnndsAt,
		string ImageName
	);
}
