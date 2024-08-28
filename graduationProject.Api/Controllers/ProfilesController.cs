using graduationProject.Api.Contracts.Customer;
using graduationProject.Api.Contracts.Provider;
using graduationProject.Api.Seeds;
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
	public class ProfilesController(IProfileService profileService, UserManager<ApplicationUser> userManager) : ControllerBase
	{
		private readonly IProfileService _profileService = profileService;
		private readonly UserManager<ApplicationUser> _userManager = userManager;

		[HttpGet("Get-Provider-from-category")]
		public async Task<IActionResult> Get([FromQuery] int providerId, CancellationToken cancellationToken)
		{
			var result = await _profileService.GetAsync(providerId, cancellationToken);

			return result.IsSuccess
				? Ok(result.Value)
				: result.ToProblem();
		}

		[HttpGet("Get-Profile")]
		public async Task<IActionResult> GetProvider(CancellationToken cancellationToken)
		{
			var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId!);
			var roles = await _userManager.GetRolesAsync(user!);

			Result<ProviderProfileResponse> provider;
			Result<CustomerResponse> customer;
			if (roles.Contains(AppRoles.Provider))
			{
				provider = await _profileService.GetProviderProfileAsync(userId,cancellationToken);
				return provider.IsSuccess
				? Ok(provider.Value)
				: provider.ToProblem();
			}
			else
			{
				customer = await _profileService.GetCustomerProfileAsync(userId, cancellationToken);

				return customer.IsSuccess
					? Ok(customer.Value)
					: customer.ToProblem();
			}

			
		}
	}
}
