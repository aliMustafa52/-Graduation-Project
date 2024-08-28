using graduationProject.Api.Contracts.Chat;
using graduationProject.Api.Services;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace graduationProject.Api.Hubs
{
	public class ChatHub(IChatService chatService,IHttpContextAccessor httpContextAccessor) : Hub
	{
		private readonly IChatService _chatService = chatService;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public override async Task OnConnectedAsync()
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "Come2Chat");
			await Clients.Caller.SendAsync("UserConnected");
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Come2Chat");

			var user = _chatService.GetUserByConnectionId(Context.ConnectionId);
			_chatService.RemoveUserFromList(user);
			await DisplayOnlineUsers();

			await base.OnDisconnectedAsync(exception);
		}

		public async Task AddUserConnectionId()
		{
			var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

			_chatService.AddUserConnectionId(userId, Context.ConnectionId);

			await DisplayOnlineUsers();
		}

		public async Task ReciveMessage(MessageRequest message)
		{
			await Clients.Group("Come2Chat").SendAsync("NewMessage", message);
		}

		public async Task CreatePrivateChat(MessageRequest message)
		{
			var privateGroupName = GetPrivateGroupName(message.From, message.To);
			await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);

			var toConnectionId = _chatService.GetConnectionIdByUser(message.To);
			await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
		}

		public async Task RecivePrivateChat(MessageRequest message)
		{
			var privateGroupName = GetPrivateGroupName(message.From, message.To);
			await Clients.Group(privateGroupName).SendAsync("NewPrivateMessage", message);
		}

		public async Task RemovePrivateChat(string from, string to)
		{
			var privateGroupName = GetPrivateGroupName(from, to);
			await Clients.Group(privateGroupName).SendAsync("ClosePrivateMessage");

			await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
		}
		private string GetPrivateGroupName(string from, string to)
		{
			var stringCompare = string.CompareOrdinal(from, to) < 0;
			return stringCompare ? $"{from}-{to}" : $"{to}-{from}";
		}
		private async Task DisplayOnlineUsers()
		{
			var onlineUsers = _chatService.GetOnlineUsers();
			await Clients.Groups("Come2Chat").SendAsync("OnlineUsers", onlineUsers);
		}
	}

}
