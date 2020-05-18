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
		private async Task RemoveUser(OnlineUserModel user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(OnlineUserModel));
			}
			onlineUsers.OnlineUsers.Remove(user);
			await onlineUsers.SaveAsync();
		}
		public string GetUserChatConnetionId(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			return onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId).ChatConnectionId;
		}
		public string GetUserNotificationConnetionId(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			return onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId).NotificationConnectionId;
		}
		public async Task SetUserNotificationConnectionId(string userId, string connectionId)
		{
			if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			user.NotificationConnectionId = connectionId;
			await onlineUsers.SaveAsync();
		}
		public async Task SetUserChatConnectionId(string userId, string connectionId)
		{
			if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(connectionId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			user.ChatConnectionId = connectionId;
			await onlineUsers.SaveAsync();
		}
		public async Task ClearChatConnectionIdOrRemoveUser(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (string.IsNullOrWhiteSpace(user.NotificationConnectionId))
			{
				await RemoveUser(user);
			}
			else
			{
				user.ChatConnectionId = null;
				await onlineUsers.SaveAsync();
			}
		}
		public async Task ClearNotificationConnectionIdOrRemoveUser(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new ArgumentNullException(nameof(userId));
			}
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (string.IsNullOrWhiteSpace(user.ChatConnectionId))
			{
				await RemoveUser(user);
			}
			else
			{
				user.NotificationConnectionId = null;
				await onlineUsers.SaveAsync();
			}
		}
	}
}