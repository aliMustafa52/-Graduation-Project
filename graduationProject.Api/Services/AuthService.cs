using graduationProject.Api.Authentication;
using graduationProject.Api.Contracts.Authentication;

namespace graduationProject.Api.Services
{
	public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider,IPasswordHasher<ApplicationUser> passwordHasher) : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IJwtProvider _jwtProvider = jwtProvider;
		private readonly Microsoft.AspNetCore.Identity.IPasswordHasher<ApplicationUser> _passwordHasher = passwordHasher;

		public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
		{
			//check if there's a user with this email
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return null;

			//check if password is correct
			var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
			if (!isPasswordCorrect)
				return null;

			//generate JWT token
			var (token, expiresIn) = _jwtProvider.GenerateToken(user);

			return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);
		}

		public async Task<AuthResponse?> RegisterAsync(RegisterRequest registerRequest)
		{
			// Check if the user already exists
			var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
			if (existingUser is not null)
				return null;

			ApplicationUser user = new()
			{
				Email = registerRequest.Email,
				UserName = registerRequest.Email,
				FirstName = registerRequest.FirstName,
				LastName = registerRequest.LastName,
			};
			
			var result =await _userManager.CreateAsync(user,registerRequest.Password);
			
			if(!result.Succeeded) 
				return null;

			//generate JWT token
			var (token, expiresIn) = _jwtProvider.GenerateToken(user);

			return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);
		}
	}
}
