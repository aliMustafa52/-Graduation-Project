namespace graduationProject.Api.Errors
{
	public static class UserErrors
	{
		public static readonly Error InvalidCredentials =
			new("User.InvalidCredentials", "Invalid Email/Password", StatusCodes.Status400BadRequest);

		public static readonly Error ExistingEmail =
			new("User.ExistingEmail", "Email Already Exists", StatusCodes.Status400BadRequest);

		public static readonly Error FieldNotFound =
			new("User.FieldNotFound", "Field Not Found", StatusCodes.Status400BadRequest);

		public static readonly Error ErrorWhileCreateing =
			new("User.ErrorWhileCreateing", "Error While Createing", StatusCodes.Status400BadRequest);

		public static readonly Error InvalidJwtToken =
			new("User.InvalidJwtToken", "Invalid Jwt Token", StatusCodes.Status400BadRequest);

		public static readonly Error InvalidRefreshToken =
			new("User.InvalidRefreshToken", "Invalid Refresh Token", StatusCodes.Status400BadRequest);
	}
}
