using System;
using System.ComponentModel.DataAnnotations;

namespace SemesterProject.ApiData.Entities
{
	public class PostLike
	{
		[Key]
		public Guid Id { get; set; }
		public Guid FromWho { get; set; }
		public DateTime WhenAdded { get; set; }
		public Guid PostId { get; set; }
	}
}
