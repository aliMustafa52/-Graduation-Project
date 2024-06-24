namespace graduationProject.Api.Seeds
{
	public static class DefaultUsers
	{
		public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
		{
			ApplicationUser admin = new()
			{
				Email = "Admin@gmail.com",
				UserName = "Admin@gmail.com",
				FirstName = "Admin",
				LastName = "Admin",
			};

			var user = await userManager.FindByNameAsync(admin.Email);
            if (user is null)
            {
                await userManager.CreateAsync(admin,"P@ssword123");
				await userManager.AddToRoleAsync(admin,AppRoles.Admin);
            }
        }
	}
}
