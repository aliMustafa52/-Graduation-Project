namespace graduationProject.Api.Services
{
	public interface IJobService
	{
		Task<Result<IEnumerable<JobResponse>>> GetAllForCurrentProviderAsync(CancellationToken cancellationToken = default);

		Task<Result<JobResponse>> GetAsync(int id, CancellationToken cancellationToken = default);

		Task<Result<JobResponse>> AddAsync(JobRequest request, CancellationToken cancellationToken = default);

		Task<Result> UpdateAsync(int id, JobRequest request, CancellationToken cancellationToken = default);

		Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
	}
}
