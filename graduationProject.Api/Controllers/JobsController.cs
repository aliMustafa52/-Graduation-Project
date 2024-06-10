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
			var job = await _jobService.GetAsync(id, cancellationToken);
			if (job is null)
				return NotFound();

			var response = job.Adapt<JobResponse>();

			return Ok(response);
		}

		[HttpPost("")]
		public async Task<IActionResult> Add([FromBody]JobRequest request, CancellationToken cancellationToken)
		{
			var job = await _jobService.AddAsync(request.Adapt<Job>(),cancellationToken);

			var response = job.Adapt<JobResponse>();

			return CreatedAtAction(nameof(Get),new {id =job.Id}, response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update([FromRoute] int id,[FromBody] JobRequest request, CancellationToken cancellationToken)
		{

			var isUpdated = await _jobService.UpdateAsync(id,request.Adapt<Job>(),cancellationToken);
			if(!isUpdated)
				return NotFound();

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{

			var isUpdated = await _jobService.DeleteAsync(id,cancellationToken);
			if (!isUpdated)
				return NotFound();

			return Ok();
		}
		[HttpPut("{id}/togglenegotiable")]
		public async Task<IActionResult> ToggleNegotiable([FromRoute] int id, CancellationToken cancellationToken)
		{
			var isToggled = await _jobService.ToggleNegotiableAsync(id, cancellationToken);
			return isToggled ? NoContent() : NotFound();
		}
	}
}
