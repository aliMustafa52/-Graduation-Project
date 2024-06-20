using graduationProject.Api.Entities;
using graduationProject.Api.Errors;
using graduationProject.Api.Persistence;
using Mapster;

namespace graduationProject.Api.Services
{
	public class JobService(ApplicationDbContext context,IImageService imageService) : IJobService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IImageService _imageService = imageService;

		public async Task<IEnumerable<JobResponse>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var jobs = await _context.Jobs.AsNoTracking().ToListAsync(cancellationToken);
			return jobs.Adapt<IEnumerable<JobResponse>>();
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

			await _context.Jobs.AddAsync(job,cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success(job.Adapt<JobResponse>());
		}
		public async Task<Result> UpdateAsync(int id, JobRequest request, CancellationToken cancellationToken = default)
		{
			var currentJob = await _context.Jobs.FindAsync(id,cancellationToken);
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
			currentJob.Price = request.Price;
			currentJob.IsNegotiable = request.IsNegotiable;
			currentJob.StartsAt = request.StartsAt;
			currentJob.EnndsAt = request.EnndsAt;

			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}

		public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
		{
			var job = await _context.Jobs.FindAsync(id,cancellationToken);
			if(job is null)
				return Result.Failure(JobsErrors.JobNotFound);

			if(job.ImageName is not null)
				_imageService.DeleteImage(job.ImageName,cancellationToken);

			_context.Jobs.Remove(job);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}

		public async Task<Result> ToggleNegotiableAsync(int id, CancellationToken cancellationToken = default)
		{
			var job = await _context.Jobs.FindAsync(id,cancellationToken);
			if (job is null)
				return Result.Failure(JobsErrors.JobNotFound);

			job.IsNegotiable = !job.IsNegotiable;
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}

		
	}
}
