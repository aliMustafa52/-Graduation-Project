using graduationProject.Api.Contracts.Offers;

namespace graduationProject.Api.Contracts.Provider
{
    public record ProviderProfileResponse(int Id, string Name, string PhoneNumber, string Governorate
        , DateOnly BirthDate, string Field, int ExperienceYears, string Address, IEnumerable<JobResponse> Jobs);
}
