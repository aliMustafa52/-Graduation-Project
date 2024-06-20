using Microsoft.EntityFrameworkCore;

namespace graduationProject.Api.Persistence.EntitiesConfigrations
{
	public class CategoryConfigrations : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.HasIndex(x => x.Title).IsUnique();

			builder.Property(x => x.Title)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(x => x.Description)
				.HasMaxLength(2000);
		}
	}
}
