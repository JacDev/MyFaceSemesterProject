
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class MessagesWithUserData
	{
		public Message Message { get; set; }
		public UserToReturnWithCounters User { get; set; }
	}
}
