using SemesterProject.ApiData.Entities;
using SemesterProject.MyFaceMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
			return onlineUsers.OnlineUsers.Any(x => x.Id == userId);
		}
		public async Task AddOnlineUser(OnlineUserModel onlineUserModel)
		{
			if (onlineUserModel != null)
			{
				onlineUsers.OnlineUsers.Add(onlineUserModel);
				await onlineUsers.SaveAsync();
			}
		}
		public OnlineUserModel GetOnlineUser(string userId)
		{
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			return user;
		}
		private async Task RemoveUser(OnlineUserModel user)
		{
			if (user!=null)
			{
				onlineUsers.OnlineUsers.Remove(user);
				await onlineUsers.SaveAsync();
			}
		}
		public string GetUserChatConnetionId(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				return null;
			}
			return onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId).ChatConnectionId;
		}
		public string GetUserNotificationConnetionId(string userId)
		{
			if (string.IsNullOrWhiteSpace(userId))
			{
				return null;
			}
			return onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId).NotificationConnectionId;
		}
		public async Task SetUserNotificationConnectionId(string userId, string connectionId)
		{
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (user != null)
			{
				user.NotificationConnectionId = connectionId;
				await onlineUsers.SaveAsync();
			}
		}
		public async Task SetUserChatConnectionId(string userId, string connectionId)
		{
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (user != null)
			{
				user.ChatConnectionId = connectionId;
				await onlineUsers.SaveAsync();
			}
		}
		public async Task ClearChatConnectionIdOrRemoveUser(string userId)
		{
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (!string.IsNullOrWhiteSpace(userId))
			{
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
		}
		public async Task ClearNotificationConnectionIdOrRemoveUser(string userId)
		{
			OnlineUserModel user = onlineUsers.OnlineUsers.FirstOrDefault(u => u.Id == userId);
			if (!string.IsNullOrWhiteSpace(userId))
			{
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
}

