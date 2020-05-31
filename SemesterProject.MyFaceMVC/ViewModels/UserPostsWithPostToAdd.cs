using SemesterProject.ApiData.Models;
using System.Collections.Generic;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class UserPostsWithPostToAdd
	{
		public List<BasicUserWithPost> UserWithPosts { get; set; } = new List<BasicUserWithPost>();
		public PostWithImageToAdd NewPost { get; set; }
		public BasicUserData BasicUser { get; set; }
	}
}
