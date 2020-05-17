using System;
using System.Collections.Generic;
using System.Text;

namespace SemesterProject.ApiData.Models
{
	public class UserToSend
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
