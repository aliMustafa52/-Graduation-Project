using graduationProject.Api.Persistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace graduationProject.Api.Services
{
	public class ChatService(ApplicationDbContext context) : IChatService
	{
		private static readonly Dictionary<string,string> Users = [];
		private readonly ApplicationDbContext _context = context;

		public bool AddUserToList(string userId)
		{
			var userToAdd = _context.Users.Where(x => x.Id == userId).Select(x => x.FirstName).SingleOrDefault();

			lock (Users)
			{
				foreach (var user in Users)
				{
					if (!Users.ContainsKey(userToAdd)) 
						return false;
				}

				Users.Add(userToAdd, null);
				return true;
			}
		}

		public void AddUserConnectionId(string userId, string connectionId)
		{
			var user = _context.Users.Where(x => x.Id == userId).Select(x => x.FirstName).SingleOrDefault();
			lock (Users)
			{
				if (Users.ContainsKey(user))
					Users[user] = connectionId;
			}
		}

		public string GetUserByConnectionId(string connectionId)
		{
			lock(Users)
			{
				return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
			}
		}

		public string GetConnectionIdByUser(string user)
		{
			lock (Users)
			{
				return Users.Where(x => x.Key == user).Select(x => x.Value).FirstOrDefault();
			}
		}

		public void RemoveUserFromList(string user)
		{
			lock (Users)
			{
				if(Users.ContainsKey(user))
					Users.Remove(user);
			}
		}

		public string[] GetOnlineUsers()
		{
			lock (Users)
			{
				return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
			}
		}
	}
}
