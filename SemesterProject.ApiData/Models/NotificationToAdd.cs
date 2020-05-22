using System;

namespace SemesterProject.ApiData.Models
{
	public class NotificationToAdd
	{
		public bool WasSeen { get; set; }
		public Guid FromWho { get; set; }
		public Guid UserId { get; set; }
		public NotificationType NotificationType { get; set; }
		public Guid EventId { get; set; }
	}
}
