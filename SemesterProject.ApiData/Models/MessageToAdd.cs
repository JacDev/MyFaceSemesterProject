using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Models
{
	public class MessageToAdd
	{
		public string Text { get; set; }
		public DateTime When { get; set; }
		public Guid ToWho { get; set; }
	}
}
