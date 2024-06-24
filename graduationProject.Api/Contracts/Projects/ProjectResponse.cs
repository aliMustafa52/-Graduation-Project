using graduationProject.Api.Contracts.Offers;

namespace graduationProject.Api.Contracts.Projects
{
	public record ProjectResponse(int Id, string Title, string Description, int CreatedById, string Status
			, string ImageName,IEnumerable<OfferResponse> Offers);
}
