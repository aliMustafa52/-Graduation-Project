using graduationProject.Api.Contracts.Categories;
using graduationProject.Api.Contracts.Provider;
using graduationProject.Api.Persistence;

namespace graduationProject.Api.Services
{
	public class CategoryService(ApplicationDbContext context, IImageService imageService) : ICategoryService
	{
		private readonly ApplicationDbContext _context = context;
		private readonly IImageService _imageService = imageService;

		public async Task<IEnumerable<CategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var categryResponses = await _context.Categories
				.Where(x => x.IsActive)
				.Include(x => x.Providers)
				.Select(x => new CategoryResponse(
					x.Id,x.Title,x.Description,x.ImageName,x.Providers.Select(p => new ProviderResponse(p.Id,p.FirstName,p.LastName))
					))
				.AsNoTracking()
				.ProjectToType<CategoryResponse>()
				.ToListAsync(cancellationToken);

			return categryResponses;
		}
		public async Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
		{
			var categroy = await _context.Categories
					.Where(x => x.Id == id && x.IsActive)
					.Include(x => x.Providers)
					.Select(x => new CategoryResponse(
						x.Id, x.Title, x.Description, x.ImageName, x.Providers
							.Select(p => new ProviderResponse(p.Id, p.FirstName, p.LastName))
						))
					.SingleOrDefaultAsync(cancellationToken);

			return categroy is null
				? Result.Failure<CategoryResponse>(CategriesErrors.CategroyNotFound)
				: Result.Success(categroy);
		}

		public async Task<Result<CategoryResponse>> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default)
		{
			var isExistingTitle = await _context.Categories.AnyAsync(x => x.Title == request.Title);
			if (isExistingTitle)
				Result.Failure<CategoryResponse>(CategriesErrors.DublicatedCategroyTitle);

			var categroy = request.Adapt<Category>();
			Result<string> imageName;
			if (request.ImageFile is not null)
			{
				imageName = await _imageService.SaveImageAsync(request.ImageFile, cancellationToken);
				categroy.ImageName = imageName.Value;
			}

			await _context.Categories.AddAsync(categroy, cancellationToken);
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success(categroy.Adapt<CategoryResponse>());
		}
		public async Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default)
		{
			var currentCategroy = await _context.Categories.Where(x => x.IsActive)
									.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
			if (currentCategroy is null)
				return Result.Failure(CategriesErrors.CategroyNotFound);

			var isExistingTitle = await _context.Categories.AnyAsync(x => x.Title == request.Title && x.Id != id);
			if (isExistingTitle)
				return Result.Failure(CategriesErrors.DublicatedCategroyTitle);

			Result<string> imageName;
			if (request.ImageFile is not null)
			{
				imageName = await _imageService.UpdateImageAsync(request.ImageFile, currentCategroy.ImageName, cancellationToken);
				currentCategroy.ImageName = imageName.Value;
			}
			currentCategroy.Title = request.Title;
			currentCategroy.Description = request.Description;

			await _context.SaveChangesAsync(cancellationToken);
			return Result.Success();
		}

		public async Task<Result> ToggleStatusAsync(int id, CancellationToken cancellationToken = default)
		{
			var categroy = await _context.Categories.FindAsync(id,cancellationToken);
										

			if (categroy is null)
				return Result.Failure(CategriesErrors.CategroyNotFound);

			categroy.IsActive = !categroy.IsActive;
			await _context.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
