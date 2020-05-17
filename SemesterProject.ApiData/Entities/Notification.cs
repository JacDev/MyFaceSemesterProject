using SemesterProject.ApiData.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SemesterProject.ApiData.Entities
{
	public class Notification
	{
		[Key]
		public Guid Id { get; set; }
		public bool WasSeen { get; set; }
		public Guid FromWho { get; set; }
		public Guid UserId { get; set; }
		public NotificationType NotificationType { get; set; }
	}
}
