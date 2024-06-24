namespace graduationProject.Api.Contracts.ContactUs
{
	public class ContactUsRequestValidator :AbstractValidator<ContactUsRequest>
	{
        public ContactUsRequestValidator()
        {
			RuleFor(x => x.Name)
			  .NotEmpty()
			  .MaximumLength(100);

			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Memo)
				.NotEmpty()
				.MaximumLength(500);

			RuleFor(x => x.AdditionalInfo)
				.MaximumLength(500);
		}
    }
}
