using graduationProject.Api.Services;
using Microsoft.AspNetCore.Http;
using graduationProject.Api.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		private readonly IAuthService _authService = authService;

		[HttpPost("")]
		public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
		{
			var authresponse = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);

			return authresponse is null ? BadRequest("Invalid Email/Password") : Ok(authresponse);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterRequest registerRequest)
		{
			var authResponse = await _authService.RegisterAsync(registerRequest);
			return authResponse is null ? BadRequest("Email already exists try to login") : Ok(authResponse);
		}
	}
}
