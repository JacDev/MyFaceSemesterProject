using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.Data;
using SemesterProject.MyFaceMVC.Repository;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Hubs
{
	public class NotificationHub : Hub
	{
		private readonly IOnlineUsersRepository _onlineUsers;
		private readonly IFriendApiAccess _friendApiAccess;
		private readonly IMessageApiAccess _messageApiService;
		private readonly IPostApiAccess _postApiAccess;
		private readonly INotificationApiAccess _notificationApiAccess;
		private readonly ILogger<NotificationHub> _logger;

		public NotificationHub(IOnlineUsersRepository onlineUsers,
			IFriendApiAccess friendApiAccess, 
			IMessageApiAccess messageApiService, 
			IPostApiAccess postApiAccess, 
			INotificationApiAccess notificationApiAccess,
			ILogger<NotificationHub> logger) 
		{
			_onlineUsers = onlineUsers;
			_friendApiAccess = friendApiAccess;
			_messageApiService = messageApiService;
			_postApiAccess = postApiAccess;
			_notificationApiAccess = notificationApiAccess;
			_logger = logger;
		}
		public async Task SendPrivateNotificaion(string toUserId, string notificationType, string fromWho, string eventId, bool wasLiked = false)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(toUserId) && !string.IsNullOrWhiteSpace(fromWho))
				{
					if (notificationType == "like")
					{
						if (!wasLiked)
						{
							await _postApiAccess.AddPostLike(fromWho, new PostLike
							{
								WhenAdded = DateTime.Now,
								FromWho = Guid.Parse(fromWho),
								PostId = Guid.Parse(eventId)
							});
							await _notificationApiAccess.AddNotification(new NotificationToAdd
							{
								FromWho = Guid.Parse(fromWho),
								UserId = Guid.Parse(toUserId),
								WasSeen = false,
								NotificationType = NotificationType.Like,
								EventId = Guid.Parse(eventId)
							});
						}
						else
						{
							await _postApiAccess.DeletePostLike(eventId, fromWho, toUserId);
							Notification notificationToDelete = await _notificationApiAccess.GetNotification(Guid.Parse(toUserId), Guid.Parse(eventId), Guid.Parse(fromWho));
							await _notificationApiAccess.DeleteNotification(toUserId, notificationToDelete.Id.ToString());
							return;
						}
					}

					else if (notificationType == "friendRequest")
					{
						if (await _friendApiAccess.CheckIfAreFriends(Guid.Parse(toUserId), Guid.Parse(fromWho)))
						{
							return;
						}
						await _notificationApiAccess.AddNotification(new NotificationToAdd
						{
							FromWho = Guid.Parse(fromWho),
							UserId = Guid.Parse(toUserId),
							WasSeen = false,
							NotificationType = NotificationType.FriendRequiest
						});
					}

					// send message if user is online
					if (_onlineUsers.IsUserOnline(toUserId))
					{
						string toWhomConnectionId = _onlineUsers.GetOnlineUser(toUserId).NotificationConnectionId;
						await Clients.Client(toWhomConnectionId).SendAsync("ReceiveNotification");
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong in NotificationHub during sending private notifications. toUserId: {toUserId}, notificationType: {notificationType}, fromWho: {fromWho}, eventId {eventId}");
				_logger.LogError($"Exception info: {ex.Message} {ex.Source}");
			}
			
		}
		public async Task SendPrivateMessage(string toUserId, string message, string fromWho)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(toUserId) && !string.IsNullOrWhiteSpace(fromWho))
				{
					// send message if user is online
					if (_onlineUsers.IsUserOnline(toUserId))
					{
						string toWhomConnectionId = _onlineUsers.GetOnlineUser(toUserId).NotificationConnectionId;
						await Clients.Client(toWhomConnectionId).SendAsync("ReceiveMessage", fromWho, message);
					}
					//if user is offline, add notification
					else
					{
						await _notificationApiAccess.AddNotification(new NotificationToAdd
						{
							FromWho = Guid.Parse(fromWho),
							UserId = Guid.Parse(toUserId),
							WasSeen = false
						});
					}
					await _messageApiService.AddMessage(fromWho, new MessageToAdd
					{
						Text = message,
						ToWho = Guid.Parse(toUserId),
						When = DateTime.Now
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Something went wrong in NotificationHub during sending private message. toUserId: {toUserId}, fromWho: {fromWho}");
				_logger.LogError($"Exception info: {ex.Message} {ex.Source}");
			}
		}
		public override async Task OnConnectedAsync()
		{
			OnlineUserModel _loggedUser = GetLoggedUser();
			if (!string.IsNullOrWhiteSpace(_loggedUser.Id))
			{
				if (_onlineUsers.IsUserOnline(_loggedUser.Id))
				{
					await _onlineUsers.RemoveUser(_loggedUser);
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
			OnlineUserModel _loggedUser = GetLoggedUser();
			if (_loggedUser != null)
			{
				await _onlineUsers.RemoveUser(_loggedUser);
			}
			await base.OnDisconnectedAsync(ex);
		}
		private OnlineUserModel GetLoggedUser()
		{
			OnlineUserModel user = new OnlineUserModel
			{
				Id = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
				FirstName = Context.User.Claims.FirstOrDefault(c => c.Type == "FirstName").Value,
				LastName = Context.User.Claims.FirstOrDefault(c => c.Type == "LastName").Value
			};
			return user;
		}
	}
}
