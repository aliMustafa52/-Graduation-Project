namespace graduationProject.Api.Contracts.Jobs
{
	public record JobRequest(
		string Title,
		string Description,
		double Price,
		bool IsNegotiable,
		DateOnly StartsAt,
		DateOnly EnndsAt
	);

}
