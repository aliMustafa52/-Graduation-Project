namespace graduationProject.Api.Errors
{
	public static class JobsErrors
	{
		public static readonly Error JobNotFound =
			new("Job.NotFount", "No Job was found with this Id", StatusCodes.Status404NotFound);
		public static readonly Error DublicatedJobTitle
				= new("Job.DublicatedJobTitle", "Another Job with the same title exists", StatusCodes.Status409Conflict);
	}
}
