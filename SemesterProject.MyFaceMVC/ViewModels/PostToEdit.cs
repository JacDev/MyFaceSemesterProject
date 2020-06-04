using System;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class PostToEdit
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string Text { get; set; }
	}
}
