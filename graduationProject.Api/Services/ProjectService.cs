using graduationProject.Api.Contracts.Offers;
using graduationProject.Api.Contracts.Projects;
using graduationProject.Api.Entities;
using graduationProject.Api.Persistence;
using System.Security.Claims;

namespace graduationProject.Api.Services
{
	public class ProjectService(ApplicationDbContext context, IImageService imageService,IHttpContextAccessor httpContextAccessor)
		: IProjectService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IImageService _imageService = imageService;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		//"Open", "Assigned", "Completed"
		public async Task<IEnumerable<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _context.Projects
					.Include(x => x.Offers)
					.Select(x => new ProjectResponse(x.Id,x.Title,x.Description,x.CreatedById,x.Status,x.ImageName,
							x.Offers.Select(o => new OfferResponse(o.Id,o.Message,o.Status))
							)
					)
					.AsNoTracking()
					.ToListAsync(cancellationToken);
		}

		public async Task<Result<IEnumerable<ProjectResponse>>> GetAllForCurrentCustomerAsync(CancellationToken cancellationToken = default)
		{
			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
							.Where(x => x.Id == userId)
							.Include(x => x.Customers)
							.ThenInclude(x => x.Projects)
							.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<IEnumerable<ProjectResponse>>(UserErrors.InvalidCredentials);

			var customer = user.Customers.FirstOrDefault();
			if (customer is null)
				return Result.Failure<IEnumerable<ProjectResponse>>(UserErrors.UserNotAssigned);

			var responses = await _context.Projects
					.Where(x => x.CreatedById == customer.Id)
					.Include(x => x.Offers)
					.Select(x => new ProjectResponse(x.Id, x.Title, x.Description, x.CreatedById, x.Status, x.ImageName,
							x.Offers.Select(o => new OfferResponse(o.Id, o.Message, o.Status))
							)
					)
					.AsNoTracking()
					.ToListAsync(cancellationToken);

			return Result.Success<IEnumerable<ProjectResponse>>(responses);
		}

		public async Task<Result<ProjectResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
		{
			var projectResponse= await _context.Projects
								.Where(x => x.Id == id)
								.Include(x => x.Offers)
								.Select(x => new ProjectResponse(x.Id, x.Title, x.Description, x.CreatedById, x.Status, x.ImageName,
										x.Offers.Select(o => new OfferResponse(o.Id, o.Message, o.Status))
										)
								)
								.SingleOrDefaultAsync(cancellationToken);

			return projectResponse is null 
				? Result.Failure<ProjectResponse>(ProjectsErrors.ProjectNotFound)
				: Result.Success(projectResponse);
		}

		public async Task<Result<ProjectResponse>> AddAsync(ProjectRequest request, CancellationToken cancellationToken = default)
		{
			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
							.Where(x => x.Id == userId)
							.Include(x => x.Customers)
							.ThenInclude(x => x.Projects)
							.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<ProjectResponse>(UserErrors.InvalidCredentials);

			var customer = user.Customers.FirstOrDefault();
			if (customer is null)
				return Result.Failure<ProjectResponse>(UserErrors.UserNotAssigned);

			var projects = customer.Projects.ToList();
			var isSameProject = projects.Contains(request.Adapt<Project>());
			if (isSameProject)
				return Result.Failure<ProjectResponse>(ProjectsErrors.DublicatedProject);

			var newProject = request.Adapt<Project>();

			Result<string> imageName;
			if (request.Image is not null)
			{
				imageName = await _imageService.SaveImageAsync(request.Image, cancellationToken);
				newProject.ImageName = imageName.Value;
			}

			newProject.CreatedById = customer.Id;
			newProject.Status = "Open";

			_context.Projects.Add(newProject);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success(newProject.Adapt<ProjectResponse>());
		}

		public async Task<Result> UpdateAsync(int id, ProjectRequest request, CancellationToken cancellationToken = default)
		{
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


			var projects = customer.Projects.Where(x => x.Id != id).ToList();
			var isSameProject = projects.Contains(request.Adapt<Project>());
			if (isSameProject)
				return Result.Failure(ProjectsErrors.DublicatedProject);

			var currentProject = await _context.Projects.FindAsync(id,cancellationToken);
			if(currentProject is null)
				return Result.Failure(ProjectsErrors.ProjectNotFound);

			//
			if (customer.Id != currentProject.CreatedById)
				return Result.Failure(ProjectsErrors.CannotEditOthersProjects);

			Result<string> imageName;
			if (request.Image is not null)
			{
				imageName = await _imageService.UpdateImageAsync(request.Image,currentProject.ImageName, cancellationToken);
				currentProject.ImageName = imageName.Value;
			}

			currentProject.Title = request.Title;
			currentProject.Description = request.Description;

			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success(currentProject.Adapt<ProjectResponse>());
		}

		public async Task<Result> ToggleStatusToAssignedAsync(int id, CancellationToken cancellationToken = default)
		{
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

			var project = await _context.Projects
				.SingleOrDefaultAsync(x => x.Id ==id && x.CreatedById == customer.Id,cancellationToken);
			if (project is null)
				return Result.Failure(ProjectsErrors.ProjectNotFound);

			project.Status = "Assigned";
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
		public async Task<Result> ToggleStatusToCompletedAsync(int id, CancellationToken cancellationToken = default)
		{
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

			var project = await _context.Projects
				.SingleOrDefaultAsync(x => x.Id == id && x.CreatedById == customer.Id, cancellationToken);
			if (project is null)
				return Result.Failure(ProjectsErrors.ProjectNotFound);

			project.Status = "Completed";
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
