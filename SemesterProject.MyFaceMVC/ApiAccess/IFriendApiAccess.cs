using SemesterProject.ApiData.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public interface IFriendApiAccess
	{
		Task<HttpResponseMessage> AddFriend(string userId, RelationToAdd relationForAdd);
		Task<bool> CheckIfAreFriends(Guid userId, Guid friendId);
		Task<HttpResponseMessage> DeleteFriend(string userId, string friendId);
		Task<List<BasicUserData>> GetFriends(string userId);
	}
}