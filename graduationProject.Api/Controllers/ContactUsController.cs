using graduationProject.Api.Contracts.ContactUs;
using graduationProject.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ContactUsController(IContactUsService contactUsService) : ControllerBase
	{
		private readonly IContactUsService _contactUsService = contactUsService;

		// GET: api/messages
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ContactUsResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			var messages = await _contactUsService.GetAllAsync(cancellationToken);
			return Ok(messages);
		}

		// GET: api/messages/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ContactUsResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
		{
			var result = await _contactUsService.GetAsync(id, cancellationToken);

			if (!result.IsSuccess)
			{
				return NotFound(result.Error);
			}

			return Ok(result.Value);
		}

		// POST: api/messages
		[HttpPost]
		public async Task<ActionResult<ContactUsResponse>> AddAsync([FromBody] ContactUsRequest request, CancellationToken cancellationToken = default)
		{
			var result = await _contactUsService.AddAsync(request, cancellationToken);

			if (!result.IsSuccess)
			{
				return BadRequest(result.Error);
			}

			return CreatedAtAction(nameof(GetAsync), new { id = result.Value.Id }, result.Value);
		}



		// DELETE: api/messages/{id}
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
		{
			var result = await _contactUsService.DeleteAsync(id, cancellationToken);

			if (!result.IsSuccess)
			{
				return NotFound(result.Error);
			}

			return NoContent();
		}
	}
}
