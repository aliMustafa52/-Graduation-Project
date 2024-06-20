namespace graduationProject.Api.Errors
{
	public static class CategriesErrors
	{
		public static readonly Error CategroyNotFound =
			new("Categroy.NotFount", "No Categroy was found with this Id", StatusCodes.Status404NotFound);
		public static readonly Error DublicatedCategroyTitle
				= new("Categroy.DublicatedCategroyTitle", "Another Categroy with the same title exists", StatusCodes.Status409Conflict);
	}
}
