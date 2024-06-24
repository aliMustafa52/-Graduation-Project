namespace graduationProject.Api.Authentication
{
	public interface IJwtProvider
	{
		(string token, int expiresIn) GenerateToken(UserSession user);
		
		string? ValidateToken(string token);
	}
}
