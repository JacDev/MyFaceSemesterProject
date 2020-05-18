using System;

namespace SemesterProject.ApiData.Models
{
	public class PostToAdd
	{
		public DateTime WhenAdded { get; set; }
		public string Text { get; set; }
		public string ImagePath { get; set; }
		public string ImageFullPath { get; set; }
	}
}
