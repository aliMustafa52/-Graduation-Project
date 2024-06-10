﻿using graduationProject.Api.Contracts.Authentication;

namespace graduationProject.Api.Services
{
	public interface IAuthService
	{
		Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);

		Task<AuthResponse?> RegisterAsync(RegisterRequest registerRequest);

		Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
		Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
	}
}
