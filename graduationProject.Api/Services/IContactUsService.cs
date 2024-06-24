using graduationProject.Api.Contracts.ContactUs;

namespace graduationProject.Api.Services
{
	public interface IContactUsService
	{
		Task<IEnumerable<ContactUsResponse>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<Result<ContactUsResponse>> GetAsync(int id, CancellationToken cancellationToken = default);

		Task<Result<ContactUsResponse>> AddAsync(ContactUsRequest request, CancellationToken cancellationToken = default);
		Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
	}
}
