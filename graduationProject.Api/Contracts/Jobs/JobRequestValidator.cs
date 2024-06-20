using FluentValidation;

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
				.NotEmpty();

			RuleFor(x => x)
				.Must(HasValidDates)
				.WithName(nameof(JobRequest.EnndsAt))
				.WithMessage("{PropertyName} must be greater than start date");

			RuleFor(x => x.ImageFile)
				.NotEmpty();
		}

		private bool HasValidDates(JobRequest request) => request.EnndsAt >= request.StartsAt;
	}
}
