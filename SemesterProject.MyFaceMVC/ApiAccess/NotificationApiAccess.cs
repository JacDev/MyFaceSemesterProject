using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public class NotificationApiAccess : INotificationApiAccess
	{
		private readonly IMyFaceApiService _myFaceApiService;

		public NotificationApiAccess(IMyFaceApiService myFaceApiService)
		{
			_myFaceApiService = myFaceApiService;
		}
		public async Task<IEnumerable<NotificationWithBasicFromWhoData>> GetNotifications(string userId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/notifications");
			return await response.ReadContentAs<List<NotificationWithBasicFromWhoData>>();
		}
		public async Task<Notification> GetNotification(Guid userId, Guid eventId, Guid friendId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/notifications/{eventId}/{friendId}");
			return await response.ReadContentAs<Notification>();
		}
		public async Task<HttpResponseMessage> AddNotification(NotificationToAdd notificationForAdd)
		{
			return await _myFaceApiService.Client.PostToApiAsJsonAsync($"api/users/{notificationForAdd.UserId}/notifications", notificationForAdd);
		}
		public async Task<HttpResponseMessage> MarkNotificationAsSeen(string userId, Guid notificationId)
		{
			return await _myFaceApiService.Client.PatchToApiAsJsonAsync($"api/users/{userId}/notifications/{notificationId}", new Notification());
		}
		public async Task<HttpResponseMessage> DeleteNotification(string userId, string notificationId)
		{
			return await _myFaceApiService.Client.DeleteFromApi($"api/users/{userId}/notifications/{notificationId}");
		}
	}
}
