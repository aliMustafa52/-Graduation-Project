namespace graduationProject.Api.Contracts.Chat
{
	public record MessageRequest(string From, string? To, string Content);
}
