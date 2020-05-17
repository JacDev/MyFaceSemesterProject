using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Models
{
	public class UserWithPost
	{
		public UserToReturn user;
		public IEnumerable<Post> Posts { get; set; }
	}
}
