using System.Diagnostics.CodeAnalysis;

namespace SemesterProject.MyFaceMVC.Data
{
	public class OnlineUserModel
	{
		[NotNull]
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[AllowNull]
		public string NotificationConnectionId { get; set; } = string.Empty;
	}
}
