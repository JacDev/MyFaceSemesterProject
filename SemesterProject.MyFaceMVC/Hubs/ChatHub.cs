using Microsoft.AspNetCore.SignalR;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Data;
using SemesterProject.MyFaceMVC.Repository;
using SemesterProject.MyFaceMVC.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Hubs
{
	public class ChatHub : Hub
	{
		private readonly IOnlineUsersRepository _onlineUsers;
		private readonly IMyFaceApiService _myFaceApiService;

		public ChatHub(IOnlineUsersRepository onlineUsers, IMyFaceApiService myFaceApiService)
		{
			_onlineUsers = onlineUsers;
			_myFaceApiService = myFaceApiService;
		}
		public async Task SendPrivateMessage(string toUserId, string message, string fromWho)
		{
			if (!string.IsNullOrWhiteSpace(toUserId) && !string.IsNullOrWhiteSpace(fromWho))
			{
				// send message if user is online
				if (_onlineUsers.IsUserOnline(toUserId))
				{
					string toWhomConnectionId = _onlineUsers.GetUserChatConnetionId(toUserId);
					await Clients.Client(toWhomConnectionId).SendAsync("ReceiveMessage", fromWho, message);
				}
				//if user is offline, add notification
				else
				{
					await _myFaceApiService.AddNotification(new NotificationToAdd
					{
						FromWho = Guid.Parse(fromWho),
						UserId = Guid.Parse(toUserId),
						WasSeen = false
					});
				}
				await _myFaceApiService.AddMessage(fromWho, new MessageToAdd
				{
					Text = message,
					ToWho = Guid.Parse(toUserId),
					When = DateTime.Now
				});
			}
		}
		public override async Task OnConnectedAsync()
		{
			var _loggedUser = GetLoggedUser();
			if (!string.IsNullOrWhiteSpace(_loggedUser.Id))
			{
				if (_onlineUsers.IsUserOnline(_loggedUser.Id))
				{
					await _onlineUsers.SetUserChatConnectionId(_loggedUser.Id, Context.ConnectionId);
				}
				else
				{
					_loggedUser.ChatConnectionId = Context.ConnectionId;
					await _onlineUsers.AddOnlineUser(_loggedUser);
				}
			}
			await base.OnConnectedAsync();
		}
		public override async Task OnDisconnectedAsync(Exception ex)
		{
			var _loggedUser = GetLoggedUser();
			await _onlineUsers.ClearChatConnectionIdOrRemoveUser(_loggedUser.Id);
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
