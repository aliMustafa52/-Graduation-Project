﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;

namespace graduationProject.Api.Persistence
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,IHttpContextAccessor httpContextAccessor)
		: IdentityDbContext<ApplicationUser>(options)
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public DbSet<Job> Jobs { get; set; }
		public DbSet<Provider> Providers { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Offer> Offers { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<Contact> Contacts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			var cascadeFks = modelBuilder.Model
								.GetEntityTypes()
								.SelectMany(t => t.GetForeignKeys())
								.Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade);
			foreach (var fk in cascadeFks)
				fk.DeleteBehavior = DeleteBehavior.Restrict;

			base.OnModelCreating(modelBuilder);
		}

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<AuditableEntity>();
			foreach (var entry in entries)
			{
				var currentUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

				if(entry.State == EntityState.Added)
				{
					entry.Property(x => x.CreatedById).CurrentValue = currentUserId;
				}
				else if(entry.State == EntityState.Modified)
				{
					entry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
					entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
				}
			}
			
			return base.SaveChangesAsync(cancellationToken);
		}
	}
}
