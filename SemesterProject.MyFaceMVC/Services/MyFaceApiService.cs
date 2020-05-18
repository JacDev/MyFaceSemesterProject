using SemesterProject.ApiData.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using SemesterProject.ApiData.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace SemesterProject.MyFaceMVC.Services
{
	public class MyFaceApiService : IMyFaceApiService
	{
		private readonly HttpClient _client;
		public MyFaceApiService(HttpClient client)
		{
			_client = client;
		}
		public async Task<List<BasicUserData>> GetFriends(string userId)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}/friends");
			return await response.ReadContentAs<List<BasicUserData>>();
		}
		public async Task<HttpResponseMessage> AddFriend(string userId, RelationToAdd relationForAdd)
		{
			return await _client.PostToApiAsJsonAsync($"api/users/{userId}/friends", relationForAdd);
		}
		public async Task<HttpResponseMessage> DeleteFriend(string userId, string friendId)
		{
			return await _client.DeleteFromApi($"api/users/{userId}/friends/{friendId}");
		}

		public async Task<Post> GetPost(string userId, string postId)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}/posts/{postId}");
			return await response.ReadContentAs<Post>();
		}
		public async Task<List<Post>> GetPosts(string userId)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}/posts");
			return await response.ReadContentAs<List<Post>>();
		}
		public async Task<HttpResponseMessage> AddPost(string userId, PostToAdd postForAdd)
		{
			return await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts", postForAdd);
		}
		public async Task<HttpResponseMessage> UpdatePost(string userId, string postId, PostToUpdate postToUpdate)
		{
			return await _client.PatchToApiAsJsonAsync($"api/users/{userId}/posts/{postId}", postToUpdate);
		}
		public async Task<HttpResponseMessage> DeletePost(string userId, string postId)
		{
			return await _client.DeleteAsync($"api/users/{userId}/posts/{postId}");
		}

		public async Task<HttpResponseMessage> AddPostLike(string userId, PostLike postLike)
		{
			return await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postLike.PostId}/likes", postLike);
		}
		public async Task<HttpResponseMessage> DeletePostLike(string postId, string fromWho, string userId)
		{
			return await _client.DeleteFromApi($"api/users/{userId}/posts/{postId}/likes/{fromWho}");
		}

		public async Task<HttpResponseMessage> AddPostComment(string userId, PostComment postComment)
		{
			return await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postComment.PostId}/comments", postComment);
		}
		public async Task<HttpResponseMessage> DeletePostComment(string postId, string commentId, string userId)
		{
			return await _client.DeleteAsync($"api/users/{userId}/posts/{postId}/comments/{commentId}");
		}

		public async Task<IEnumerable<NotificationWithBasicFromWhoData>> GetNotifications(string userId)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}/notifications");
			return await response.ReadContentAs<List<NotificationWithBasicFromWhoData>>();
		}
		public async Task<HttpResponseMessage> AddNotification(NotificationToAdd notificationForAdd)
		{
			return await _client.PostToApiAsJsonAsync($"api/users/{notificationForAdd.UserId}/notifications", notificationForAdd);
		}
		public async Task<HttpResponseMessage> MarkNotificationAsSeen(string userId, Guid notificationId)
		{
			return await _client.PatchToApiAsJsonAsync($"api/users/{userId}/notifications/{notificationId}", new Notification());
		}

		public async Task<IEnumerable<Message>> GetMessagesWith(string userId, string friendId)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}/messages/{friendId}");
			return await response.ReadContentAs<List<Message>>();
		}
		public async Task<IEnumerable<Message>> GetMessages(string userId)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}/messages");
			return await response.ReadContentAs<List<Message>>();
		}
		public async Task<HttpResponseMessage> AddMessage(string userId, MessageToAdd messageForAdd)
		{
			return await _client.PostToApiAsJsonAsync($"api/users/{userId}/messages", messageForAdd);
		}

		public async Task<UserToReturnWithCounters> GetUser(string userId)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}");
			return await response.ReadContentAs<UserToReturnWithCounters>();
		}
		public async Task<HttpResponseMessage> AddUserIfNotExist(ClaimsPrincipal userPrincipal)
		{
			BasicUserData userToSend = new BasicUserData
			{
				Id = Guid.Parse(userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
				FirstName = userPrincipal.Claims.FirstOrDefault(x => x.Type == "FirstName").Value,
				LastName = userPrincipal.Claims.FirstOrDefault(x => x.Type == "LastName").Value,
			};
			return await _client.PostToApiAsJsonAsync("api/users", userToSend);
		}
		public async Task<List<BasicUserData>> GetFoundUsers(string searchName)
		{
			HttpResponseMessage response = await _client.GetAsync($"api/users?searchName={searchName}");
			return await response.ReadContentAs<List<BasicUserData>>();
		}
	}
}