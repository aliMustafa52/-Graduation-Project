namespace graduationProject.Api.Errors
{
	public static class ProviderErrors
	{
		public static readonly Error ProviderNotFound =
			new("Provider.NotFound", "No Provider was found with this Id", StatusCodes.Status404NotFound);
	}
}
