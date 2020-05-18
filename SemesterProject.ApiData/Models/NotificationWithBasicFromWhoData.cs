using SemesterProject.ApiData.Entities;

namespace SemesterProject.ApiData.Models
{
	public class NotificationWithBasicFromWhoData
	{
		public Notification Notification { get; set; }
		public BasicUserData User { get; set; }
	}
}
