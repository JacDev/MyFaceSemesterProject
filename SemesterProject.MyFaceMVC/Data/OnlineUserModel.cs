using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Data
{
	public class OnlineUserModel
	{
		[NotNull]
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[AllowNull]
		public string ChatConnectionId { get; set; } = string.Empty;
		[AllowNull]
		public string NotificationConnectionId { get; set; } = string.Empty;
	}
}
