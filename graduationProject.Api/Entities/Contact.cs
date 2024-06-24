namespace graduationProject.Api.Entities
{
	public class Contact
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Memo { get; set; } = string.Empty;
		public string AdditionalInfo { get; set; } = string.Empty;
	}
}
