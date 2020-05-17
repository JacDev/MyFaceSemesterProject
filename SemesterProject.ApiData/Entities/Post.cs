using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SemesterProject.ApiData.Entities
{
	public class Post
	{
		[Key]
		public Guid Id { get; set; }
		public DateTime WhenAdded { get; set; }
		[Required]
		public string Text { get; set; }
		public Guid UserId { get; set; }
		public string ImagePath { get; set; }
		public string ImageFullPath { get; set; }
		public virtual ICollection<PostLike> PostLikes { get; set; }
		public virtual ICollection<PostComment> PostComments { get; set; }
		public Post() 
		{
			PostLikes = new List<PostLike>();
			PostComments = new List<PostComment>();
		}
	}
}
