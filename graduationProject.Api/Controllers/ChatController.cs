using graduationProject.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace graduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ChatController(IChatService chatService) : ControllerBase
	{
		private readonly IChatService _chatService = chatService;

		[HttpPost("register-user")]
		public IActionResult RegisterUSer()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			return _chatService.AddUserToList(userId) ? Ok() : BadRequest("this name is taken");
		}
	}
}
