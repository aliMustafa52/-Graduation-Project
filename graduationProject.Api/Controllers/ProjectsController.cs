using graduationProject.Api.Contracts.Categories;
using graduationProject.Api.Contracts.Projects;
using graduationProject.Api.Seeds;
using graduationProject.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class ProjectsController(IProjectService projectService) : ControllerBase
	{
		private readonly IProjectService _projectService = projectService;

		[HttpGet("")]
		[Authorize(Roles = AppRoles.Provider)]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var categories = await _projectService.GetAllAsync(cancellationToken);

			return Ok(categories);
		}

		[HttpGet("current-customer")]
		[Authorize(Roles = AppRoles.Customer)]
		public async Task<IActionResult> GetAllForCurrentCustomer(CancellationToken cancellationToken)
		{
			var categories = await _projectService.GetAllForCurrentCustomerAsync(cancellationToken);

			return Ok(categories);
		}

		[HttpGet("{id}")]
		[Authorize(Roles = $"{AppRoles.Provider},{AppRoles.Customer}")]
		public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
		{
			var result = await _projectService.GetAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok(result.Value)
				: result.ToProblem();
		}

		[HttpPost("")]
		[Authorize(Roles = AppRoles.Customer)]
		public async Task<IActionResult> Add([FromForm] ProjectRequest request, CancellationToken cancellationToken)
		{
			var result = await _projectService.AddAsync(request, cancellationToken);

			return result.IsSuccess
				? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
				: result.ToProblem();
		}

		[HttpPut("{id}")]
		[Authorize(Roles = AppRoles.Customer)]
		public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ProjectRequest request, CancellationToken cancellationToken)
		{

			var result = await _projectService.UpdateAsync(id, request, cancellationToken);

			return result.IsSuccess
				? NoContent()
				: result.ToProblem();
		}

		[HttpPut("{id}/toggle-to-assigned")]
		[Authorize(Roles = AppRoles.Customer)]
		public async Task<IActionResult> ToggleStatusToAssigned([FromRoute] int id, CancellationToken cancellationToken)
		{

			var result = await _projectService.ToggleStatusToAssignedAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok()
				: result.ToProblem();
		}

		[HttpPut("{id}/toggle-to-completed")]
		[Authorize(Roles = AppRoles.Customer)]
		public async Task<IActionResult> ToggleStatusToCompleted([FromRoute] int id, CancellationToken cancellationToken)
		{

			var result = await _projectService.ToggleStatusToCompletedAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok()
				: result.ToProblem();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = AppRoles.Customer)]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{

			var result = await _projectService.DeleteAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok()
				: result.ToProblem();
		}
	}
}
