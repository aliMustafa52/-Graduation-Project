using graduationProject.Api.Contracts.Jobs;
using graduationProject.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class JobsController(IJobService jobService) : ControllerBase
	{
		private readonly IJobService _jobService = jobService;

		[HttpGet("")]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var jobs =await _jobService.GetAllAsync(cancellationToken);
			var responses = jobs.Adapt<IEnumerable<JobResponse>>();

			return Ok(responses);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get([FromRoute]int id, CancellationToken cancellationToken)
		{
			var result = await _jobService.GetAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok(result.Value) 
				: result.ToProblem();
		}

		[HttpPost("")]
		public async Task<IActionResult> Add([FromForm]JobRequest request, CancellationToken cancellationToken)
		{
			var result = await _jobService.AddAsync(request,cancellationToken);

			return result.IsSuccess 
				? CreatedAtAction(nameof(Get),new {id =result.Value.Id}, result.Value)
				:result.ToProblem();
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id,[FromForm] JobRequest request, CancellationToken cancellationToken)
		{

			var result = await _jobService.UpdateAsync(id,request,cancellationToken);

			return result.IsSuccess
				? NoContent()
				:result.ToProblem();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{

			var result = await _jobService.DeleteAsync(id,cancellationToken);

			return result.IsSuccess
				? Ok()
				: result.ToProblem();
		}
		[HttpPut("{id}/togglenegotiable")]
		public async Task<IActionResult> ToggleNegotiable([FromRoute] int id, CancellationToken cancellationToken)
		{
			var result = await _jobService.ToggleNegotiableAsync(id, cancellationToken);

			return result.IsSuccess 
				? NoContent() 
				: result.ToProblem();
		}
	}
}
