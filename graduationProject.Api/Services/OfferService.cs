using graduationProject.Api.Contracts.Offers;
using graduationProject.Api.Errors;
using graduationProject.Api.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace graduationProject.Api.Services
{
	public class OfferService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : IOfferService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public async Task<Result<IEnumerable<OfferResponse>>> GetAllAsync(int projectId, CancellationToken cancellationToken = default)
		{
			var isExistingProject = await _context.Projects.AnyAsync(x => x.Id == projectId, cancellationToken);
			if (!isExistingProject)
				return Result.Failure<IEnumerable<OfferResponse>>(ProjectsErrors.ProjectNotFound);

			var offers = await _context.Offers
						.Where(x => x.ProjectId == projectId)
						.Select(x => new OfferResponse(x.Id,x.Message, x.Status, x.ProviderId
							, $"{x.Provider.FirstName} {x.Provider.LastName}"))
						.AsNoTracking()
						.ToListAsync(cancellationToken);


			

			return Result.Success<IEnumerable<OfferResponse>>(offers);
		}

		public async Task<Result<OfferResponse>> GetAsync(int projectId, int id, CancellationToken cancellationToken = default)
		{
			var isExistingProject = await _context.Projects.AnyAsync(x => x.Id == projectId, cancellationToken);
			if (!isExistingProject)
				return Result.Failure<OfferResponse>(ProjectsErrors.ProjectNotFound);

			var offer = await  _context.Offers
				.Where(x => x.Id == id)
				.Select(x => new OfferResponse(x.Id, x.Message, x.Status, x.ProviderId
												   , $"{x.Provider.FirstName} {x.Provider.LastName}"))
				.AsNoTracking()
				.SingleOrDefaultAsync(cancellationToken);

			if (offer is null)
				return Result.Failure<OfferResponse>(OffersErrors.OfferNotFound);

			return Result.Success(offer);
		}

		public async Task<Result<OfferResponse>> AddAsync(int projectId, OfferRequest request, CancellationToken cancellationToken = default)
		{
			var project = await _context.Projects
							.Where(x => x.Id == projectId)
							.Include(x => x.Offers)
							.SingleOrDefaultAsync(cancellationToken);
			if(project is null)
				return Result.Failure<OfferResponse>(ProjectsErrors.ProjectNotFound);

			if(project.Status != "Open")
				return Result.Failure<OfferResponse>(ProjectsErrors.ProjectAssignedOrCompleted);

			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
						.Where(x => x.Id == userId)
						.Include(x => x.Providers)
						.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<OfferResponse>(UserErrors.InvalidCredentials);

			var provider = user.Providers.FirstOrDefault();
			if (project.Offers.Any(o => o.ProviderId == provider!.Id))
				return Result.Failure<OfferResponse>(OffersErrors.DuplicateOffer);

			Offer offer = new() { Message = request.Message, ProjectId = projectId, ProviderId = provider!.Id, Status = "Pending" };
			project.Offers.Add(offer);

			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success(offer.Adapt<OfferResponse>());
		}

		public async Task<Result> ToggleStatusToAcceptedAsync(int projectId, int id, CancellationToken cancellationToken = default)
		{
			var project = await _context.Projects
							.Where(x => x.Id == projectId)
							.Include(x => x.Offers)
							.SingleOrDefaultAsync(cancellationToken);
			if (project is null)
				return Result.Failure<OfferResponse>(ProjectsErrors.ProjectNotFound);

			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
							.Where(x => x.Id == userId)
							.Include(x => x.Customers)
							.ThenInclude(x => x.Projects)
							.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure(UserErrors.InvalidCredentials);

			var customer = user.Customers.FirstOrDefault();
			if (customer is null)
				return Result.Failure(UserErrors.UserNotAssigned);

			if(!customer.Projects.Contains(project))
				return Result.Failure(ProjectsErrors.CannotEditOthersProjects);

			var offer = await _context.Offers.FindAsync(id,cancellationToken);
			if(offer is null)
				return Result.Failure(OffersErrors.OfferNotFound);

			offer.Status = "Accepted";
			project.Status = "Assigned";

			foreach (var item in project.Offers)
			{
				if (item != offer)
					item.Status = "Rejected";
			}

			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}
	}
}
