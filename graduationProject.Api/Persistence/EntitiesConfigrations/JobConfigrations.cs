
namespace graduationProject.Api.Persistence.EntitiesConfigrations
{
	public class JobConfigrations : IEntityTypeConfiguration<Job>
	{
		public void Configure(EntityTypeBuilder<Job> builder)
		{

			builder.Property(x => x.Title)
				.IsRequired()
				.HasMaxLength(200);

			builder.Property(x => x.Description)
				.HasMaxLength(2000);
		}
	}
}
