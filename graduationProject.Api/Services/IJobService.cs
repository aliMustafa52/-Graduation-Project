namespace graduationProject.Api.Services
{
	public interface IJobService
	{
		Task<IEnumerable<JobResponse>> GetAllAsync(CancellationToken cancellationToken = default);

		Task<Result<JobResponse>> GetAsync(int id, CancellationToken cancellationToken = default);

		Task<Result<JobResponse>> AddAsync(JobRequest request, CancellationToken cancellationToken = default);

		Task<Result> UpdateAsync(int id, JobRequest request, CancellationToken cancellationToken = default);

		Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
		Task<Result> ToggleNegotiableAsync(int id, CancellationToken cancellationToken = default);
	}
}
