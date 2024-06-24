using FluentValidation;

namespace graduationProject.Api.Contracts.Offers
{
	public class OfferRequestValidator : AbstractValidator<Offer>
	{
        public OfferRequestValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty();
        }
    }
}
