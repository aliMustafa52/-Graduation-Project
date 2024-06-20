using graduationProject.Api.Contracts.Provider;

namespace graduationProject.Api.Contracts.Categories
{
	public record CategoryResponse(int Id,string Title, string Description, string ImageName
		,IEnumerable<ProviderResponse> Providers);
}
