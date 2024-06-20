namespace graduationProject.Api.Contracts.Categories
{
	public record CategoryRequest(string Title, string Description, IFormFile? ImageFile);

}
