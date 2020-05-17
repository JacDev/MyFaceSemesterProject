using System;
using System.ComponentModel.DataAnnotations;

namespace SemesterProject.ApiData.Entities
{
	public class Message
	{
		[Key]
		public Guid Id { get; set; }
		public string Text { get; set; }
		public DateTime When { get; set; }
		public Guid FromWho { get; set; }
		public Guid ToWho { get; set; }
	}
}
