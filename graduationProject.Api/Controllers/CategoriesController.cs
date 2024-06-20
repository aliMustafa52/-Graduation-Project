using graduationProject.Api.Contracts.Categories;
using graduationProject.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CategoriesController(ICategoryService categoryService) : ControllerBase
	{
		private readonly ICategoryService _categoryService = categoryService;

		[HttpGet("")]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var categories = await _categoryService.GetAllAsync(cancellationToken);

			return Ok(categories);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
		{
			var result = await _categoryService.GetAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok(result.Value)
				: result.ToProblem();
		}

		[HttpPost("")]
		public async Task<IActionResult> Add([FromForm] CategoryRequest request, CancellationToken cancellationToken)
		{
			var result = await _categoryService.AddAsync(request, cancellationToken);

			return result.IsSuccess
				? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value)
				: result.ToProblem();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CategoryRequest request, CancellationToken cancellationToken)
		{

			var result = await _categoryService.UpdateAsync(id, request, cancellationToken);

			return result.IsSuccess
				? NoContent()
				: result.ToProblem();
		}

		[HttpPut("{id}/delete")]
		public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken)
		{

			var result = await _categoryService.ToggleStatusAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok()
				: result.ToProblem();
		}
	}
}
