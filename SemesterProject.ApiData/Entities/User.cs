using System;
using System.Collections.Generic;

namespace SemesterProject.ApiData.Entities
{
	public class User
	{
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Relation> Relations { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public User()
        {
            Relations = new List<Relation>();
            Posts = new List<Post>();
            Notifications = new List<Notification>();
        }
    }
}