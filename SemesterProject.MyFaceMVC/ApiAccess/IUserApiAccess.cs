using SemesterProject.ApiData.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public interface IUserApiAccess
	{
		Task<HttpResponseMessage> AddProfilePic(string userId, string imagePath);
		Task<HttpResponseMessage> AddUserIfNotExist(ClaimsPrincipal userPrincipal);
		Task<List<BasicUserData>> GetFoundUsers(string searchName);
		Task<UserToReturnWithCounters> GetUser(string userId);
	}
}