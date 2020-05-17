using SemesterProject.ApiData.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public interface INotificationRepository
	{
		IQueryable<Notification> GetUserNotifications(Guid userId);
		Task AddNotificationAsync(Notification notification);
		Task DeleteNotificationAsync(Guid notificationId);
		Notification GetNotification(Guid notificationId);
		Task MarkNotificationAsSeen(Guid notificationId);
	}
}
