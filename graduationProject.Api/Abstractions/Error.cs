namespace graduationProject.Api.Abstractions
{
	public record Error(string Code, string Description,int? StausCode)
	{
		public static readonly Error None = new Error(string.Empty,string.Empty,null);
	}
}
