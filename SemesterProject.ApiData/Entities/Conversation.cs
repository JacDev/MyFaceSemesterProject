using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace SemesterProject.ApiData.Entities
{
	public class Conversation
	{
		[Key]
		public Guid Id { get; set; }
		public Guid FirstUser { get; set; }
		public Guid SecondUser { get; set; }
		public ICollection<Message> Messages { get; set; }
		public Conversation()
		{
			Messages = new List<Message>();
		}
	}
}
