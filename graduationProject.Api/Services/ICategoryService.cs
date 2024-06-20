using graduationProject.Api.Contracts.Categories;

namespace graduationProject.Api.Services
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

		Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default);

		Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default);

		Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default);

		Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default);
	}
}
