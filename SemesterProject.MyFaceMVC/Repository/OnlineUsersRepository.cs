using SemesterProject.MyFaceMVC.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Repository
{
	public class OnlineUsersRepository : IOnlineUsersRepository
	{
		private readonly IOnlineUsers onlineUsers;
		public OnlineUsersRepository(IOnlineUsers mVCDbContext)
		{
			onlineUsers = mVCDbContext;
		}
		public bool IsUserOnline(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			return onlineUsers.OnlineUsers.Any(x => x.Id == userId);
		}
		public async Task AddOnlineUser(OnlineUserModel onlineUserModel)
		{
			if (onlineUserModel == null)
			{
				throw new ArgumentNullException(nameof(onlineUserModel));
			}
			onlineUsers.OnlineUsers.Add(onlineUserModel);
			await onlineUsers.SaveAsync();
		}
		public OnlineUserModel GetOnlineUser(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			return onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId); ;
		}
		public async Task RemoveUser(OnlineUserModel user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			onlineUsers.OnlineUsers.Remove(user);
			await onlineUsers.SaveAsync();
		}
	}
}