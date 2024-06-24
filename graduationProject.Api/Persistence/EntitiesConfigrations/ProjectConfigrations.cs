using Microsoft.EntityFrameworkCore;

namespace graduationProject.Api.Persistence.EntitiesConfigrations
{
	public class ProjectConfigrations : IEntityTypeConfiguration<Project>
	{
		public void Configure(EntityTypeBuilder<Project> builder)
		{
			builder.Property(x => x.Title).HasMaxLength(200);
			builder.Property(x => x.Description).HasMaxLength(2000);
		}
	}
}
