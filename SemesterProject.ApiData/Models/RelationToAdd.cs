using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Models
{
	public class RelationToAdd
	{
		public Guid FriendId { get; set; }
		public DateTime SinceWhen { get; set; }
	}
}
