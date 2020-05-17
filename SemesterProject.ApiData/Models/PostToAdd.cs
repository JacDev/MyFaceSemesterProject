using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
