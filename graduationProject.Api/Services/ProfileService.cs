using graduationProject.Api.Contracts.Customer;
using graduationProject.Api.Contracts.Offers;
using graduationProject.Api.Contracts.Provider;
using graduationProject.Api.Entities;
using graduationProject.Api.Persistence;
using System.Security.Claims;

namespace graduationProject.Api.Services
{
	public class ProfileService(ApplicationDbContext context, IHttpContextAccessor httpContext) : IProfileService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IHttpContextAccessor _httpContext = httpContext;

		public async Task<Result<ProviderProfileResponse>> GetAsync(int providerId, CancellationToken cancellationToken = default)
		{
			var provider = await _context.Providers
					.Where(x => x.Id == providerId)
					.Include(x => x.Jobs)
					.Select(x => new ProviderProfileResponse(x.Id,$"{x.FirstName} {x.LastName}" ,x.PhoneNumber,
						x.Governorate,x.BirthDate, x.Field,x.ExperienceYears, x.Address,
						x.Jobs.Select(job => new JobResponse(job.Id,job.Title,job.Description,job.StartsAt,job.EnndsAt,job.ImageName))
						)
					)
					.AsNoTracking()
					.SingleOrDefaultAsync(cancellationToken);
			if (provider == null)
				Result.Failure<ProviderProfileResponse>(ProviderErrors.ProviderNotFound);

			return Result.Success<ProviderProfileResponse>(provider);
		}

		public async Task<Result<ProviderProfileResponse>> GetProviderProfileAsync(string userId,CancellationToken cancellationToken = default)
		{
			var user = await _context.Users
						.Where(x => x.Id == userId)
						.Include(x => x.Providers)
						.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<ProviderProfileResponse>(UserErrors.InvalidCredentials);
			var provider = user.Providers.FirstOrDefault();

			var providerResponse = await _context.Providers
					.Where(x => x.Id == provider!.Id)
					.Include(x => x.Jobs)
					.Select(x => new ProviderProfileResponse(x.Id, $"{x.FirstName} {x.LastName}", x.PhoneNumber,
						x.Governorate, x.BirthDate, x.Field, x.ExperienceYears, x.Address,
						x.Jobs.Select(job => new JobResponse(job.Id, job.Title, job.Description, job.StartsAt, job.EnndsAt, job.ImageName))
						)
					)
					.AsNoTracking()
					.SingleOrDefaultAsync(cancellationToken);
			if (provider == null)
				Result.Failure<ProviderProfileResponse>(ProviderErrors.ProviderNotFound);

			return Result.Success<ProviderProfileResponse>(providerResponse!);
		}

		public async Task<Result<CustomerResponse>> GetCustomerProfileAsync(string userId, CancellationToken cancellationToken = default)
		{
			var user = await _context.Users
						.Where(x => x.Id == userId)
						.Include(x => x.Customers)
						.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<CustomerResponse>(UserErrors.InvalidCredentials);

			var customer = user.Customers.FirstOrDefault();

			var customerResponse = await _context.Providers
					.Where(x => x.Id == customer!.Id)
					.Select(x => new CustomerResponse(x.Id, $"{x.FirstName} {x.LastName}", x.PhoneNumber,
						x.Governorate, x.BirthDate, x.Address
						)
					)
					.AsNoTracking()
					.SingleOrDefaultAsync(cancellationToken);
			if (customerResponse == null)
				Result.Failure<ProviderProfileResponse>(ProviderErrors.ProviderNotFound);

			return Result.Success<CustomerResponse>(customerResponse!);
		}
	}
}
