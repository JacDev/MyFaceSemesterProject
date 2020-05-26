using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public class FriendApiAccess : IFriendApiAccess
	{
		private readonly IMyFaceApiService _myFaceApiService;

		public FriendApiAccess(IMyFaceApiService myFaceApiService)
		{
			_myFaceApiService = myFaceApiService;
		}
		public async Task<List<BasicUserData>> GetFriends(string userId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetAsync($"api/users/{userId}/friends");
			return await response.ReadContentAs<List<BasicUserData>>();
		}
		public async Task<HttpResponseMessage> AddFriend(string userId, RelationToAdd relationForAdd)
		{
			return await _myFaceApiService.Client.PostToApiAsJsonAsync($"api/users/{userId}/friends", relationForAdd);
		}
		public async Task<HttpResponseMessage> DeleteFriend(string userId, string friendId)
		{
			return await _myFaceApiService.Client.DeleteFromApi($"api/users/{userId}/friends/{friendId}");
		}
		public async Task<bool> CheckIfAreFriends(Guid userId, Guid friendId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/friends/{friendId}");
			return await response.ReadContentAs<bool>();
		}
	}
}
