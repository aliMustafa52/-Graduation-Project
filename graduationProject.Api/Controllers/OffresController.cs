using graduationProject.Api.Contracts.Offers;
using graduationProject.Api.Contracts.Projects;
using graduationProject.Api.Seeds;
using graduationProject.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.Api.Controllers
{
	[Route("api/projects/{projectId}/[controller]")]
	[ApiController]
	[Authorize()]
	public class OffresController(IOfferService offerService) : ControllerBase
	{
		private readonly IOfferService _offerService = offerService;

		[HttpGet("")]
		public async Task<IActionResult> GetAll([FromRoute]int projectId, CancellationToken cancellationToken)
		{
			var result = await _offerService.GetAllAsync(projectId,cancellationToken);

			return result.IsSuccess
				? Ok(result.Value)
				: result.ToProblem();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] int projectId, [FromRoute] int id, CancellationToken cancellationToken)
		{
			var result = await _offerService.GetAsync(projectId,id, cancellationToken);

			return result.IsSuccess
				? Ok(result.Value)
				: result.ToProblem();
		}

		[HttpPost("")]
		[Authorize(Roles = AppRoles.Provider)]
		public async Task<IActionResult> Add([FromRoute] int projectId, [FromBody] OfferRequest request, CancellationToken cancellationToken)
		{
			var result = await _offerService.AddAsync(projectId, request, cancellationToken);

			return result.IsSuccess
				? CreatedAtAction(nameof(Get), new { projectId,id = result.Value.Id }, result.Value)
				: result.ToProblem();
		}

		[HttpPut("{id}/toggle-to-accepted")]
		[Authorize(Roles = AppRoles.Customer)]
		public async Task<IActionResult> ToggleStatusToAccepted([FromRoute] int projectId, [FromRoute] int id, CancellationToken cancellationToken)
		{

			var result = await _offerService.ToggleStatusToAcceptedAsync(projectId,id, cancellationToken);

			return result.IsSuccess
				? Ok()
				: result.ToProblem();
		}
	}
}
