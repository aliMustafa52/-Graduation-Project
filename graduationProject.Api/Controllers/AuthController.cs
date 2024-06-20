using graduationProject.Api.Services;
using Microsoft.AspNetCore.Http;
using graduationProject.Api.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace graduationProject.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService,UserManager<ApplicationUser> userManager) : ControllerBase
	{
		private readonly IAuthService _authService = authService;
		private readonly UserManager<ApplicationUser> _userManager = userManager;

		[HttpPost("")]
		public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
		{
			var authResponseResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

			return authResponseResult.IsSuccess
				? Ok(authResponseResult.Value)
				: authResponseResult.ToProblem();
		}


		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterRequest registerRequest)
		{
			var authResponseResult = await _authService.RegisterAsync(registerRequest);

			return authResponseResult.IsSuccess
				? Ok(authResponseResult.Value)
				: authResponseResult.ToProblem();
		}


		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
		{
			var authResponseResult = await _authService.GetRefreshTokenAsync(refreshTokenRequest.Token
						, refreshTokenRequest.RefreshToken, cancellationToken);

			return authResponseResult.IsSuccess
				? Ok(authResponseResult.Value)
				: authResponseResult.ToProblem();
		}

		[HttpPost("revoke-refresh-token")]
		public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
		{
			var result = await _authService.RevokeRefreshTokenAsync(refreshTokenRequest.Token
						, refreshTokenRequest.RefreshToken, cancellationToken);

			return result.IsSuccess 
				? Ok() 
				: result.ToProblem();
		}




	}
}
