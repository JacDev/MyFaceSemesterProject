using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public class UserApiAccess : IUserApiAccess
	{
		private readonly IMyFaceApiService _myFaceApiService;

		public UserApiAccess(IMyFaceApiService myFaceApiService)
		{
			_myFaceApiService = myFaceApiService;
		}
		public async Task <HttpResponseMessage> AddProfilePic(string userId, string imagePath)
		{
			return await _myFaceApiService.Client.PostToApiAsJsonAsync($"api/users/{userId}/{imagePath}", "");
		}
		public async Task<HttpResponseMessage> AddUserIfNotExist(ClaimsPrincipal userPrincipal)
		{
			BasicUserData userToSend = new BasicUserData
			{
				Id = Guid.Parse(userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
				FirstName = userPrincipal.Claims.FirstOrDefault(x => x.Type == "FirstName").Value,
				LastName = userPrincipal.Claims.FirstOrDefault(x => x.Type == "LastName").Value,
			};
			return await _myFaceApiService.Client.PostToApiAsJsonAsync("api/users", userToSend);
		}
		public async Task<List<BasicUserData>> GetFoundUsers(string searchName)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/with/{searchName}");
			return await response.ReadContentAs<List<BasicUserData>>();
		}
		public async Task<UserToReturnWithCounters> GetUser(string userId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}");
			return await response.ReadContentAs<UserToReturnWithCounters>();
		}
	}
}
