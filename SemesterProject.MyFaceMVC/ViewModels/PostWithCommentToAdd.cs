using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SemesterProject.MyFaceMVC.ViewModels
{
	public class PostWithCommentToAdd
	{
		public Post Post { get; set; }
		[Required]
		[NotNull]
		public string Text { get; set; }
		public IEnumerable<BasicUserData> Users { get; set; }
		public BasicUserData User { get; set; }
	}
}
