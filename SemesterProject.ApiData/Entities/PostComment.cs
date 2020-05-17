using System;
using System.ComponentModel.DataAnnotations;

namespace SemesterProject.ApiData.Entities
{
	public class PostComment
	{
		[Key]
		public Guid Id { get; set; }
		public Guid FromWho { get; set; }
		public string Text { get; set; }
		public DateTime WhenAdded { get; set; }
		public Guid PostId { get; set; }	
	}
}
