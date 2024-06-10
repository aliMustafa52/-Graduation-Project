namespace graduationProject.Api.Contracts.Jobs
{
	public class JobRequestValidator :AbstractValidator<JobRequest>
	{
        public JobRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));

            RuleFor(x => x.EnndsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(x => x.StartsAt);
        }
    }
}
