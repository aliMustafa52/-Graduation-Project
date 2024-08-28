using graduationProject.Api.Abstractions;
using graduationProject.Api.Contracts.Jobs;
using graduationProject.Api.Seeds;
using graduationProject.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace graduationProject.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = AppRoles.Provider)]
	public class JobsController(IJobService jobService) : ControllerBase
	{
		private readonly IJobService _jobService = jobService;

		[HttpGet("")]
		public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
		{
			var jobResponses = await _jobService.GetAllForCurrentProviderAsync(cancellationToken);

			return jobResponses.IsSuccess
				? Ok(jobResponses.Value)
				: jobResponses.ToProblem();
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
				? Ok(result.Value)
				:result.ToProblem();
		}

		[HttpPut("{id}")]

		public async Task<IActionResult> Update([FromRoute] int id, [FromForm] JobRequest request, CancellationToken cancellationToken)
		{

			var result = await _jobService.UpdateAsync(id,request,cancellationToken);

			return result.IsSuccess
				? Ok()
				:result.ToProblem();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
		{

			var result = await _jobService.DeleteAsync(id, cancellationToken);

			return result.IsSuccess
				? Ok()
				: result.ToProblem();
		}
		
	}
}
