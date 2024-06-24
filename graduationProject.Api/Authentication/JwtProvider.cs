using graduationProject.Api.Seeds;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace graduationProject.Api.Authentication
{
	public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
	{
		private readonly JwtOptions _jwtOptions = jwtOptions.Value;

		public (string token, int expiresIn) GenerateToken(UserSession user)
		{
			Claim[] claims = [
				new(ClaimTypes.NameIdentifier,user.Id),
				new(ClaimTypes.Email,user.Email!),
				new(ClaimTypes.GivenName,$"{user.FirstName} {user.LastName}"),
				new(ClaimTypes.Role,user.Role),
				new("Jti",Guid.NewGuid().ToString())
			];

			var expiresIn = _jwtOptions.ExpiryMinutes;

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtOptions.Issuer,
				audience: _jwtOptions.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expiresIn),
				signingCredentials: signingCredentials
			);

			return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn * 60);
		}

		public string? ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					IssuerSigningKey = symmetricSecurityKey,
					ValidateIssuerSigningKey = true,
					ValidateIssuer =false,
					ValidateAudience = false,
					ClockSkew =TimeSpan.Zero
				},out SecurityToken validatedToken);

				var jwtSecurityToken = (JwtSecurityToken) validatedToken;
				return jwtSecurityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
			}
			catch
			{
				return null;
			}
		}
	}
}
