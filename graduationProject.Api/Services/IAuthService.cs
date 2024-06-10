using graduationProject.Api.Contracts.Authentication;

namespace graduationProject.Api.Services
{
	public interface IAuthService
	{
		Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);

		Task<AuthResponse?> RegisterAsync(RegisterRequest registerRequest);
	}
}
