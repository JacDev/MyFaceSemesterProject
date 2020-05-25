using Microsoft.AspNetCore.Http;
using System;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class PostWithImageToAdd
	{
		public DateTime WhenAdded { get; set; }
		public string Text { get; set; }
		public string ImagePath { get; set; }
		public string ImageFullPath { get; set; }
		public IFormFile Picture { get; set; } = null;
	}
}
