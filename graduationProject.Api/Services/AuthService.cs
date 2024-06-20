using Azure;
using graduationProject.Api.Authentication;
using graduationProject.Api.Contracts.Authentication;
using graduationProject.Api.Errors;
using graduationProject.Api.Persistence;
using Microsoft.AspNetCore.Identity;
using System.Numerics;
using System.Security.Cryptography;

namespace graduationProject.Api.Services
{
	public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider
		,RoleManager<IdentityRole> roleManager,ApplicationDbContext context) : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IJwtProvider _jwtProvider = jwtProvider;
		private readonly RoleManager<IdentityRole> _roleManager = roleManager;
		private readonly ApplicationDbContext _context = context;
		private readonly int _refreshTokenExpiryDays = 14;


		public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
		{
			//check if there's a user with this email
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

			//check if password is correct
			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
			if (!isPasswordCorrect)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

			//generate JWT token
			var (token, expiresIn) = _jwtProvider.GenerateToken(user);

			//generate Refresh token
			var refreshtoken = GenerateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

			//Add Refresh Token to the database
			user.RefreshTokens.Add(new RefreshToken
			{
				Token = refreshtoken,
				ExpiresOn = refreshTokenExpiration
			});

			await _userManager.UpdateAsync(user);
			var userRole = await userManager.GetRolesAsync(user);

			var authResponse = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, userRole.First(), token, expiresIn
					, refreshtoken, refreshTokenExpiration);

			return Result.Success(authResponse);
		}

		public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest registerRequest)
		{
			// Check if the user already exists
			var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
			if (existingUser is not null)
				return Result.Failure<AuthResponse>(UserErrors.ExistingEmail);

			
			//
			 await AddAdminIfNeeded();



			ApplicationUser user = new()
			{
				Email = registerRequest.Email,
				UserName = registerRequest.Email,
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName,
				BirthDate = registerRequest.BirthDate,
				TimeAddUser = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time"))
			};

			var result = await _userManager.CreateAsync(user, registerRequest.Password);

			if (!result.Succeeded)
				return Result.Failure<AuthResponse>(UserErrors.ErrorWhileCreateing);

			var lastUserId = await GetLastId();

			if (lastUserId == null)
			{
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
			}

			if (registerRequest.IsProvider)
			{
				var isAdded = AddProvider(registerRequest, lastUserId);
				if (!isAdded)
					return Result.Failure<AuthResponse>(UserErrors.FieldNotFound);
			}
			else if (registerRequest.IsCustomer)
			{
				AddCustomer(registerRequest, lastUserId);
			}

			//Assign Default Role : Admin to first registrar; rest is user
			var checkAdmin = await _roleManager.FindByNameAsync("Admin");
			if (checkAdmin is null)
			{
				await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
				await userManager.AddToRoleAsync(user, "Admin");
			}
			else if (registerRequest.IsProvider)
			{
				var checkUser = await roleManager.FindByNameAsync("Provider");
				if (checkUser is null)
					await roleManager.CreateAsync(new IdentityRole() { Name = "Provider" });


				await userManager.AddToRoleAsync(user, "Provider");

			}
			else if (registerRequest.IsCustomer)
			{
				var checkUser = await roleManager.FindByNameAsync("Customer");
				if (checkUser is null)
					await roleManager.CreateAsync(new IdentityRole() { Name = "Customer" });

				await userManager.AddToRoleAsync(user, "Customer");
			}
			//
			//generate JWT token
			var (token, expiresIn) = _jwtProvider.GenerateToken(user);

			//generate Refresh token
			var refreshtoken = GenerateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

			//Add Refresh Token to the database
			user.RefreshTokens.Add(new RefreshToken
			{
				Token = refreshtoken,
				ExpiresOn = refreshTokenExpiration
			});

			await _userManager.UpdateAsync(user);

			var userRole = await userManager.GetRolesAsync(user);
			var authResponse = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, userRole.First(), token, expiresIn
					, refreshtoken, refreshTokenExpiration);

			return Result.Success(authResponse);
		}

		public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
		{
			var userId = _jwtProvider.ValidateToken(token);
			if (userId is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var user = await _userManager.FindByIdAsync(userId);
			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
			if (userRefreshToken is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;

			//generate JWT token
			var (newJwtToken, expiresIn) = _jwtProvider.GenerateToken(user);

			//generate Refresh token
			var newRefreshtoken = GenerateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

			//Add Refresh Token to the database
			user.RefreshTokens.Add(new RefreshToken
			{
				Token = newRefreshtoken,
				ExpiresOn = refreshTokenExpiration
			});

			await _userManager.UpdateAsync(user);

			var userRole = await userManager.GetRolesAsync(user);
			var authResponse = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName,userRole.First(), newJwtToken, expiresIn
					, newRefreshtoken, refreshTokenExpiration);

			return Result.Success(authResponse);
		}

		public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
		{
			var userId = _jwtProvider.ValidateToken(token);
			if (userId is null)
				return Result.Failure(UserErrors.InvalidJwtToken);

			var user = await _userManager.FindByIdAsync(userId);
			if (user is null)
				return Result.Failure(UserErrors.InvalidJwtToken);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
			if (userRefreshToken is null)
				return Result.Failure(UserErrors.InvalidRefreshToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;

			await _userManager.UpdateAsync(user);

			return Result.Success();
		}


		private static string GenerateRefreshToken()
		{
			var number = RandomNumberGenerator.GetBytes(64);
			var token = Convert.ToBase64String(number);

			return token;
		}
		private async Task<bool> AddAdminIfNeeded()
		{

			if (_context.Users.Count() == 1)
			{
				var admin = await _userManager.Users.FirstOrDefaultAsync();
				if (admin != null)
				{
					admin.FirstName = "Admin";
					admin.LastName = "Admin";
					await _userManager.UpdateAsync(admin);
					await _context.SaveChangesAsync();
					return true; // Admin added successfully
				}
			}

			return false; // Admin not added
		}
		private async Task<string?> GetLastId()
		{
			return await _context.Users
				.Where(n => n.FirstName != "Admin")
				.OrderByDescending(u => u.TimeAddUser)
				.Select(u => u.Id)
				.FirstOrDefaultAsync();
		}

		private bool AddProvider(RegisterRequest registerRequest, string userId)
		{

			var provider = new Provider
			{
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName,
				PhoneNumber = registerRequest.PhoneNumber,
				Email = registerRequest.Email,
				BirthDate = registerRequest.BirthDate,
				Governorate = registerRequest.Governorate,
				Field = registerRequest.Field,
				Address =registerRequest.Address,
				ExperienceYears = registerRequest.ExperienceYears,
				UserId = userId,

			};
			
			//add category
			var category = _context.Categories.SingleOrDefault(x => x.Title == registerRequest.Field);
			if (category is null) 
				return false;

			provider.CategoryId =category.Id;

			category.Providers.Add(provider);

			_context.Providers.Add(provider);
			return true;
		}
		private void AddCustomer(RegisterRequest registerRequest, string userId)
		{

			var customer = new Customer
			{
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName,
				PhoneNumber = registerRequest.PhoneNumber,
				Email = registerRequest.Email,
				BirthDate = registerRequest.BirthDate,
				Governorate = registerRequest.Governorate,
				Address = registerRequest.Address,
				UserId = userId,

			};
			_context.Customers.Add(customer);
		}

	}
}
