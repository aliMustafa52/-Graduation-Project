namespace graduationProject.Api.Contracts.Authentication
{
	public record AuthResponse
		(
			string Id,
			string? Email,
			string FirstName,
			string LastName,
			string? Role,
			string Token,
			int Expiresin,
			string RefreshToken,
			DateTime RefreshTokenExpiration
		);
}
