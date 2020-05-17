using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SemesterProject.MyFaceMVC.Models
{
	public class PostWithCommentToAdd
	{
		public Post Post { get; set; }
		[Required]
		[NotNull]
		public string Text { get; set; }
		public IEnumerable<UserToReturn> Users { get; set; }
	}
}
