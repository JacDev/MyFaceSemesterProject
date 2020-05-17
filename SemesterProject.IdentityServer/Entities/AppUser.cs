using Microsoft.AspNetCore.Identity;
using System;

namespace SemesterProject.IdentityServer.Entities
{
	public class AppUser : IdentityUser<Guid>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirht { get; set; }
	}
}
