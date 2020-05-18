using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System.Collections.Generic;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class UserPostsWithPostToAdd
	{
		public IEnumerable<Post> Posts { get; set; }
		public PostToAdd NewPost { get; set; }

	}
}
