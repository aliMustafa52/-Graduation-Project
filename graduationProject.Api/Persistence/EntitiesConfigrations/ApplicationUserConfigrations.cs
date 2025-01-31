﻿using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace graduationProject.Api.Persistence.EntitiesConfigrations
{
	public class ApplicationUserConfigrations : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(x => x.FirstName).HasMaxLength(100);
			builder.Property(x => x.LastName).HasMaxLength(100);

			builder
				.OwnsMany(x => x.RefreshTokens)
				.ToTable("RefreshTokens")
				.WithOwner()
				.HasForeignKey("UserId");

			//users types

			
		}
	}
}
