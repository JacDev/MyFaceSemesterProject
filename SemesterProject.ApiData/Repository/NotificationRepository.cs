using SemesterProject.ApiData.AppDbContext;
using SemesterProject.ApiData.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly IApiDbContext _appDbContext;
		public NotificationRepository(IApiDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		public async Task AddNotificationAsync(Notification notification)
		{
			if (notification == null)
			{
				throw new ArgumentNullException(nameof(notification));
			}
			if (notification.FromWho == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(notification.FromWho));
			}
			await _appDbContext.Notifications.AddAsync(notification);
			await _appDbContext.SaveAsync();
		}

		public async Task DeleteNotificationAsync(Guid notificationId)
		{
			if (notificationId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(notificationId));
			}

			var notificationToDelete = _appDbContext.Notifications.FirstOrDefault(
				s => s.Id == notificationId);
			if (notificationToDelete == null)
			{
				throw new ArgumentNullException(nameof(notificationToDelete));
			}
			_appDbContext.Notifications.Remove(notificationToDelete);
			await _appDbContext.SaveAsync();
		}

		public Notification GetNotification(Guid notificationId)
		{
			if (notificationId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(notificationId));
			}
			var notificationToDelete = _appDbContext.Notifications.FirstOrDefault(s => s.Id == notificationId);
			if (notificationToDelete == null)
			{
				throw new ArgumentNullException(nameof(notificationToDelete));
			}
			return notificationToDelete;
		}

		public IQueryable<Notification> GetUserNotifications(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			return _appDbContext.Notifications.Where(s => s.UserId == userId);
		}

		public async Task MarkNotificationAsSeen(Guid notificationId)
		{
			Notification notification = _appDbContext.Notifications.FirstOrDefault(n => n.Id == notificationId);
			notification.WasSeen = true;
			await _appDbContext.SaveAsync();
		}
	}
}
