namespace graduationProject.Api.Services
{
	public interface IJobService
	{
		Task<IEnumerable<Job>> GetAllAsync(CancellationToken cancellationToken = default);

		Task<Job?> GetAsync(int id, CancellationToken cancellationToken = default);

		Task<Job> AddAsync(Job job, CancellationToken cancellationToken = default);

		Task<bool> UpdateAsync(int id, Job job, CancellationToken cancellationToken = default);

		Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
		Task<bool> ToggleNegotiableAsync(int id, CancellationToken cancellationToken = default);
	}
}
