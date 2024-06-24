namespace graduationProject.Api.Contracts.Projects
{
	public class ProjectRequestValidator :AbstractValidator<ProjectRequest>
	{
        public ProjectRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(3, 2000);
        }
    }
}
