namespace graduationProject.Api.Contracts.Projects
{
	public record ProjectRequest(string Title, string Description,IFormFile Image);
}
