using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Models
{
	public class NotificationToAdd
	{
		public bool WasSeen { get; set; }
		public Guid FromWho { get; set; }
		public Guid UserId { get; set; }
		public NotificationType NotificationType { get; set; }
	}
}
