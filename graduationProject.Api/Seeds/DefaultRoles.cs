﻿namespace graduationProject.Api.Seeds
{
	public static class DefaultRoles
	{
		public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole(AppRoles.Admin));
				await roleManager.CreateAsync(new IdentityRole(AppRoles.Provider));
				await roleManager.CreateAsync(new IdentityRole(AppRoles.Customer));
			}
		}
	}
}
