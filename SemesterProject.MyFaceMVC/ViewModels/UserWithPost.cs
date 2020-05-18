using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System.Collections.Generic;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class UserWithPost
	{
		public UserToReturnWithCounters user;
		public IEnumerable<Post> Posts { get; set; }
	}
}
