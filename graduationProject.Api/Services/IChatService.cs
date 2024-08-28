namespace graduationProject.Api.Services
{
	public interface IChatService
	{
		bool AddUserToList(string userId);

		void AddUserConnectionId(string userId, string connectionId);

		string GetUserByConnectionId(string connectionId);

		string GetConnectionIdByUser(string user);

		void RemoveUserFromList(string user);

		string[] GetOnlineUsers();
	}
}
