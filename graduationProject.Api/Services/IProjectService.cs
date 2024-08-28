using graduationProject.Api.Contracts.Categories;
using graduationProject.Api.Contracts.Projects;

namespace graduationProject.Api.Services
{
	public interface IProjectService
	{
		Task<IEnumerable<ProjectResponse>> GetAllAsync(CancellationToken cancellationToken = default);
		Task<Result<IEnumerable<ProjectResponse>>> GetAllForCurrentCustomerAsync(CancellationToken cancellationToken = default);
		Task<Result<ProjectResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
		Task<Result<ProjectResponse>> AddAsync(ProjectRequest request, CancellationToken cancellationToken = default);
		Task<Result> UpdateAsync(int id, ProjectRequest request, CancellationToken cancellationToken = default);
		Task<Result> ToggleStatusToAssignedAsync(int id, CancellationToken cancellationToken = default);
		Task<Result> ToggleStatusToCompletedAsync(int id, CancellationToken cancellationToken = default);
		Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
	}
}
