namespace graduationProject.Api.Contracts.Authentication
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
        public RegisterRequestValidator()
        {
			RuleFor(x => x.FirstName)
				.NotEmpty()
                .Matches("^[a-zA-Z]+$")
                .WithMessage("{PropertyName} must contain only English characters");

			RuleFor(x => x.LastName)
				.NotEmpty()
                .Matches("^[a-zA-Z]+$")
				.WithMessage("{PropertyName} must contain only English characters");

			RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .Matches("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()])[a-zA-Z0-9!@#$%^&*()]{8,32}$")
				.WithMessage("{PropertyName} must contain at least one digit, one lowercase letter, one uppercase letter, one special character, minimum of 8 characters and maximum of 32 characters");

			RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);

            RuleFor(x => x.Field)
				.NotNull()
                .NotEmpty()
                .When(x => x.IsProvider);

			RuleFor(x => x.ExperienceYears)
				.NotNull()
				.NotEmpty()
				.When(x => x.IsProvider);
		}
    }
}
