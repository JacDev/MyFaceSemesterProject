using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class BasicUserWithPost
	{
		public BasicUserData User { get; set; }
		public Post Post { get; set; }
	}
}
