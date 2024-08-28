using graduationProject.Api.Contracts.Customer;
using graduationProject.Api.Contracts.Provider;

namespace graduationProject.Api.Services
{
	public interface IProfileService
	{
		Task<Result<ProviderProfileResponse>> GetAsync(int providerId, CancellationToken cancellationToken = default);
		Task<Result<ProviderProfileResponse>> GetProviderProfileAsync(string userId, CancellationToken cancellationToken = default);
		Task<Result<CustomerResponse>> GetCustomerProfileAsync(string userId, CancellationToken cancellationToken = default);
	}
}
