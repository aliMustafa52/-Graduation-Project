using graduationProject.Api.Contracts.Projects;
using graduationProject.Api.Entities;
using graduationProject.Api.Errors;
using graduationProject.Api.Persistence;
using Mapster;
using System.Security.Claims;

namespace graduationProject.Api.Services
{
	public class JobService(ApplicationDbContext context,IImageService imageService,IHttpContextAccessor httpContextAccessor)
		: IJobService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IImageService _imageService = imageService;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public async Task<Result<IEnumerable<JobResponse>>> GetAllForCurrentProviderAsync(CancellationToken cancellationToken = default)
		{
			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
							.Where(x => x.Id == userId)
							.Include(x => x.Providers)
							.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<IEnumerable<JobResponse>>(UserErrors.InvalidCredentials);

			var provider = user.Providers.FirstOrDefault();
			if (provider is null)
				return Result.Failure<IEnumerable<JobResponse>>(UserErrors.UserNotAssigned);

			var jobs = await _context.Jobs
					.Where(x => x.ProviderId == provider.Id)
					.AsNoTracking()
					.ToListAsync(cancellationToken);
			return Result.Success(jobs.Adapt<IEnumerable<JobResponse>>());
		}
		public async Task<Result<JobResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
		{
			var job = await _context.Jobs.FindAsync(id, cancellationToken);

			return job is null 
				? Result.Failure<JobResponse>(JobsErrors.JobNotFound)
				: Result.Success(job.Adapt<JobResponse>());
		}
		public async Task<Result<JobResponse>> AddAsync(JobRequest request, CancellationToken cancellationToken = default)
		{
			var isExistingTitle = await _context.Jobs.AnyAsync(x => x.Title == request.Title);
			if (isExistingTitle)
				Result.Failure<JobResponse>(JobsErrors.DublicatedJobTitle);

			var job = request.Adapt<Job>();
			Result<string> imageName;
			if (request.ImageFile is not null)
			{
				 imageName =await _imageService.SaveImageAsync(request.ImageFile,cancellationToken);
				job.ImageName = imageName.Value;
			}

			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
							.Where(x => x.Id == userId)
							.Include(x => x.Providers)
							.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<JobResponse>(UserErrors.InvalidCredentials);

			var provider = user.Providers.FirstOrDefault();
			if (provider is null)
				return Result.Failure<JobResponse>(UserErrors.UserNotAssigned);

			job.ProviderId = provider.Id;
			provider.Jobs.Add(job);

			await _context.Jobs.AddAsync(job,cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success(job.Adapt<JobResponse>());
		}
		public async Task<Result> UpdateAsync(int id, JobRequest request, CancellationToken cancellationToken = default)
		{

			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
							.Where(x => x.Id == userId)
							.Include(x => x.Providers)
							.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<JobResponse>(UserErrors.InvalidCredentials);

			var provider = user.Providers.FirstOrDefault();
			if (provider is null)
				return Result.Failure<JobResponse>(UserErrors.UserNotAssigned);

			var currentJob = await _context.Jobs
					.SingleOrDefaultAsync(x => x.Id == id && x.ProviderId == provider.Id, cancellationToken);
			if (currentJob is null)
				return Result.Failure(JobsErrors.JobNotFound);

			var isExistingTitle = await _context.Jobs.AnyAsync(x => x.Title == request.Title && x.Id != id);
			if(isExistingTitle)
				return Result.Failure(JobsErrors.DublicatedJobTitle);

			Result<string> imageName;
			if (request.ImageFile is not null)
			{
				imageName = await _imageService.UpdateImageAsync(request.ImageFile,currentJob.ImageName,cancellationToken);
				currentJob.ImageName = imageName.Value;
			}
			currentJob.Title = request.Title;
			currentJob.Description = request.Description;
			currentJob.StartsAt = request.StartsAt;
			currentJob.EnndsAt = request.EnndsAt;

			
			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}

		public async Task<Result> DeleteAsync(int id,  CancellationToken cancellationToken = default)
		{
			var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _context.Users
							.Where(x => x.Id == userId)
							.Include(x => x.Providers)
							.SingleOrDefaultAsync(cancellationToken);
			if (user is null)
				return Result.Failure<JobResponse>(UserErrors.InvalidCredentials);

			var provider = user.Providers.FirstOrDefault();
			if (provider is null)
				return Result.Failure<JobResponse>(UserErrors.UserNotAssigned);

			var job = await _context.Jobs
					.SingleOrDefaultAsync(x => x.Id == id && x.ProviderId == provider.Id, cancellationToken);
			if (job is null)
				return Result.Failure(JobsErrors.JobNotFound);

			if (job.ImageName is not null)
				_imageService.DeleteImage(job.ImageName,cancellationToken);

			_context.Jobs.Remove(job);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}

		

		
	}
}
