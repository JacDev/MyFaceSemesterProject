using Microsoft.AspNetCore.SignalR;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Data;
using SemesterProject.MyFaceMVC.Repository;
using SemesterProject.MyFaceMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Hubs
{
	public class NotificationHub : Hub
	{
		private readonly IOnlineUsersRepository _onlineUsers;
		private readonly IMyFaceApiService _myFaceApiService;

		public NotificationHub(IOnlineUsersRepository onlineUsers, IMyFaceApiService myFaceApiService)
		{
			_onlineUsers = onlineUsers;
			_myFaceApiService = myFaceApiService;
		}
		public async Task SendPrivateNotificaion(string toUserId, string message, string fromWho, string postId, bool wasLiked)
		{
			if (!string.IsNullOrWhiteSpace(toUserId) && !string.IsNullOrWhiteSpace(fromWho))
			{
				// send message if user is online
				if (_onlineUsers.IsUserOnline(toUserId))
				{
					string toWhomConnectionId = _onlineUsers.GetUserNotificationConnetionId(toUserId);
					await Clients.Client(toWhomConnectionId).SendAsync("ReceiveNotification");
				}
				await _myFaceApiService.AddNotification(toUserId, new NotificationToAdd
				{
					FromWho = Guid.Parse(fromWho),
					UserId = Guid.Parse(toUserId),
					WasSeen = false
				});
				if (message == "like")
				{
					if (!wasLiked)
					{
						await _myFaceApiService.AddPostLike(fromWho, new PostLike
						{
							WhenAdded = DateTime.Now,
							FromWho = Guid.Parse(fromWho),
							PostId = Guid.Parse(postId)
						});
					}
					else
					{
						await _myFaceApiService.DeletePostLike(postId, fromWho, toUserId);
					}
				}
			}
		}
		public override async Task OnConnectedAsync()
		{
			var _loggedUser = GetLoggedUser();
			if (!string.IsNullOrWhiteSpace(_loggedUser.Id))
			{
				if (_onlineUsers.IsUserOnline(_loggedUser.Id))
				{
					await _onlineUsers.SetUserNotificationConnectionId(_loggedUser.Id, Context.ConnectionId);
				}
				else
				{
					_loggedUser.NotificationConnectionId = Context.ConnectionId;
					await _onlineUsers.AddOnlineUser(_loggedUser);
				}

			}
			await base.OnConnectedAsync();
		}
		public override async Task OnDisconnectedAsync(Exception ex)
		{
			var _loggedUser = GetLoggedUser();
			await _onlineUsers.ClearNotificationConnectionIdOrRemoveUser(_loggedUser.Id);
			await base.OnDisconnectedAsync(ex);
		}
		private OnlineUserModel GetLoggedUser()
		{
			var user = new OnlineUserModel
			{
				Id = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
				FirstName = Context.User.Claims.FirstOrDefault(c => c.Type == "FirstName").Value,
				LastName = Context.User.Claims.FirstOrDefault(c => c.Type == "LastName").Value
			};
			return user;
		}
	}
}
