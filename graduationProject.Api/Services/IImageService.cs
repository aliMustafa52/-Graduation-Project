namespace graduationProject.Api.Services
{
	public interface IImageService
	{
		Task<Result<string>> SaveImageAsync(IFormFile file,CancellationToken cancellationToken = default);
		Task<Result<string>> UpdateImageAsync(IFormFile file,string? imageName, CancellationToken cancellationToken = default);
		Result DeleteImage(string imageName, CancellationToken cancellationToken = default);
		
	}
}
