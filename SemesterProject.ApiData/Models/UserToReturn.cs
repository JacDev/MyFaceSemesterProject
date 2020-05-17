using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Models
{
	public class UserToReturn
	{
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NewNotificationsCounter { get; set; }
        public int FriendsCounter { get; set; }
        public int PostCounter { get; set; }
    }
}
