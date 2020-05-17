using System;

namespace SemesterProject.ApiData.Entities
{
	public class Relation
	{
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public DateTime SinceWhen { get; set; }
    }
}
