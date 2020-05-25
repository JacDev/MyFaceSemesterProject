using SemesterProject.ApiData.Entities;
using System.Collections.Generic;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class UserPostsWithPostToAdd
	{
		public IEnumerable<Post> Posts { get; set; }
		public PostWithImageToAdd NewPost { get; set; }

	}
}
