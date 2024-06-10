using graduationProject.Api.Persistence;

namespace graduationProject.Api.Services
{
	public class JobService(ApplicationDbContext context) : IJobService
	{
		private readonly ApplicationDbContext _context = context;

		public async Task<IEnumerable<Job>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return await _context.Jobs.AsNoTracking().ToListAsync(cancellationToken);
		}
		public async Task<Job?> GetAsync(int id, CancellationToken cancellationToken = default)
		{
			return await _context.Jobs.FindAsync(id);
		}
		public async Task<Job> AddAsync(Job job, CancellationToken cancellationToken = default)
		{
			await _context.Jobs.AddAsync(job,cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return job;
		}
		public async Task<bool> UpdateAsync(int id, Job job, CancellationToken cancellationToken = default)
		{
			var currentJob = await _context.Jobs.FindAsync(id);
			if (currentJob is null)
				return false;

			currentJob.Title = job.Title;
			currentJob.Description = job.Description;
			currentJob.Price = job.Price;
			currentJob.IsNegotiable = job.IsNegotiable;
			currentJob.StartsAt = job.StartsAt;
			currentJob.EnndsAt = job.EnndsAt;

			await _context.SaveChangesAsync(cancellationToken);
			return true;
		}

		public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
		{
			var job = await _context.Jobs.FindAsync(id);
			if(job is null)
				return false;

			_context.Jobs.Remove(job);
			await _context.SaveChangesAsync(cancellationToken);

			return true;
		}

		public async Task<bool> ToggleNegotiableAsync(int id, CancellationToken cancellationToken = default)
		{
			var job = await _context.Jobs.FindAsync(id);
			if (job is null)
				return false;

			job.IsNegotiable = !job.IsNegotiable;
			await _context.SaveChangesAsync(cancellationToken);

			return true;
		}

		
	}
}
