namespace graduationProject.Api.Services
{
	public class ImageService(IWebHostEnvironment webHostEnvironment) : IImageService
	{
		private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
		private readonly List<string> _allowedExtensions = [".jpg", ".jpeg", ".png"];
		private readonly int _maxAllowedSize = 2097152;


		public async Task<Result<string>> SaveImageAsync(IFormFile file, CancellationToken cancellationToken = default)
		{
			var extension = Path.GetExtension(file.FileName);
			if (!_allowedExtensions.Contains(extension))
				Result.Failure(ImageErrors.NotAllowedExtension);

			if(file.Length > _maxAllowedSize)
				Result.Failure(ImageErrors.ExceedMaxSize);

			var imageName = $"{Guid.NewGuid()}{extension}";

			var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images", imageName);
			using var stream = File.Create(path);
			await file.CopyToAsync(stream, cancellationToken);

			return Result.Success(imageName);
		}

		public async Task<Result<string>> UpdateImageAsync(IFormFile file, string? imageName, CancellationToken cancellationToken = default)
		{

			if(imageName is not null)
			{
				var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images", imageName);

				if (File.Exists(oldImagePath))
					File.Delete(oldImagePath);
			}

			var extension = Path.GetExtension(file.FileName);
			if (!_allowedExtensions.Contains(extension))
				Result.Failure(ImageErrors.NotAllowedExtension);

			if (file.Length > _maxAllowedSize)
				Result.Failure(ImageErrors.ExceedMaxSize);

			var newImageName = $"{Guid.NewGuid()}{extension}";

			var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images", newImageName);
			using var stream = File.Create(path);
			await file.CopyToAsync(stream, cancellationToken);

			return Result.Success(newImageName);
		}
		public Result DeleteImage(string imageName, CancellationToken cancellationToken = default)
		{
			var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images", imageName);

			if (File.Exists(oldImagePath))
				File.Delete(oldImagePath);

			return Result.Success();
		}
	}
}
