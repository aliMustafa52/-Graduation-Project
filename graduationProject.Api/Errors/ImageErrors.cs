namespace graduationProject.Api.Errors
{
	public static class ImageErrors
	{
		public static readonly Error NotAllowedExtension =
			new("Image.NotAllowedExtension", "Only .png, .jpg, .jpeg files are allowed!", StatusCodes.Status400BadRequest);

		public static readonly Error ExceedMaxSize =
			new("Image.ExceedMaxSize",  "File cannot be more that 2 MB!", StatusCodes.Status400BadRequest);
	}
}
