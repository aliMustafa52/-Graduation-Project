using graduationProject.Api.Contracts.Offers;
using graduationProject.Api.Contracts.Projects;

namespace graduationProject.Api.Services
{
	public interface IOfferService
	{
		Task<Result<IEnumerable<OfferResponse>>> GetAllAsync(int projectId, CancellationToken cancellationToken = default);

		Task<Result<OfferResponse>> GetAsync(int projectId, int id, CancellationToken cancellationToken = default);
		Task<Result<OfferResponse>> AddAsync(int projectId, OfferRequest request, CancellationToken cancellationToken = default);
		Task<Result> ToggleStatusToAcceptedAsync(int projectId, int id, CancellationToken cancellationToken = default);
	}
}
