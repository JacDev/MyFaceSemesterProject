using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public interface INotificationApiAccess
	{
		Task<HttpResponseMessage> AddNotification(NotificationToAdd notificationForAdd);
		Task<HttpResponseMessage> DeleteNotification(string userId, string notificationId);
		Task<Notification> GetNotification(Guid userId, Guid eventId, Guid friendId);
		Task<IEnumerable<NotificationWithBasicFromWhoData>> GetNotifications(string userId);
		Task<HttpResponseMessage> MarkNotificationAsSeen(string userId, Guid notificationId);
	}
}