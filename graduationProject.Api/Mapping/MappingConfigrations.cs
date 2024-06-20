using graduationProject.Api.Contracts.Categories;

namespace graduationProject.Api.Mapping
{
	public class MappingConfigrations : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			//config.NewConfig<Category, CategoryResponse>()
			//	.Map(dest => dest.Providers, src => src.Providers.Select(provider => new Answer { Content = answer }));
		}
	}
}
