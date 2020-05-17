using IdentityModel.Client;
using SemesterProject.ApiData.Models;
using System;
using System.Collections;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using SemesterProject.ApiData.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SemesterProject.IdentityServer.Entities;
using System.Security.Claims;
using System.Text.Json;

namespace SemesterProject.MyFaceMVC.Services
{
	public class MyFaceApiService : IMyFaceApiService
	{
		private readonly HttpClient _client;


		public MyFaceApiService(HttpClient client)
		{
			_client = client;
		}
		public async Task AddFriendsRelation(string userId, RelationToAdd relationForAdd)
		{
			var response = await _client.PostToApiAsJsonAsync($"api/users/{userId}/friends", relationForAdd);

		}

		public async Task AddMessage(string userId, MessageToAdd messageForAdd)
		{
			var response = await _client.PostToApiAsJsonAsync($"api/users/{userId}/messages", messageForAdd);
		}
		public async Task<IEnumerable<Message>> GetMessagesWith(string userId, string friendId)
		{
			var response = await _client.GetAsync($"api/users/{userId}/messages/{friendId}");
			return await response.ReadContentAs<List<Message>>();
		}

		public async Task AddNotification(string userId, NotificationToAdd notificationForAdd)
		{
			var response = await _client.PostToApiAsJsonAsync($"api/users/{notificationForAdd.UserId}/notifications", notificationForAdd);
		}
		public async Task<Post> GetPost(string userId, string postId)
		{
			var response = await _client.GetAsync($"api/users/{userId}/posts/{postId}");
			return await response.ReadContentAs<Post>();
		}

		public async Task AddPost(string userId, PostToAdd postForAdd)
		{
			var response = await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts", postForAdd);
		}

		public async Task<IEnumerable<NotificationWithBasicFromWhoData>> GetNotifications(string userId)
		{
			var response = await _client.GetAsync($"api/users/{userId}/notifications");
			return await response.ReadContentAs<List<NotificationWithBasicFromWhoData>>();
		}

		public async Task DeleteFriendsRelation(string userId, string friendId)
		{
			var response = await _client.DeleteFromApi($"api/users/{userId}/friends/{friendId}");
		}

		public async Task<List<UserToReturnAsFriend>> GetFriends(string userId)
		{
			var response = await _client.GetAsync($"api/users/{userId}/friends");
			return await response.ReadContentAs<List<UserToReturnAsFriend>>();
		}
		public async Task<UserToReturn> GetUser(string userId)
		{
			var response = await _client.GetAsync($"api/users/{userId}");
			return await response.ReadContentAs<UserToReturn>();
		}

		public async Task<List<Post>> GetPosts(string userId)
		{
			 var response = await _client.GetAsync($"api/users/{userId}/posts");
			return await response.ReadContentAs<List<Post>>();
		}
		public async Task DeletePost(string userId, string postId)
		{
			var response = await _client.DeleteAsync($"api/users/{userId}/posts/{postId}");
		}

		public async Task MarkNotificationAsSeen(string userId, Guid notificationId)
		{
			
			var response = await _client.PatchToApiAsJsonAsync($"api/users/{userId}/notifications/{notificationId}", new Notification());
		}

		public async Task UpdatePost(string userId, string postId, PostToUpdate postToUpdate)
		{
			var response = await _client.PatchToApiAsJsonAsync($"api/users/{userId}/posts/{postId}", postToUpdate);
		}

		public async Task AddUserIfNotExist(ClaimsPrincipal principal)
		{
			await _client.RegisterUser(principal);
		}
		public async Task<List<UserToReturn>> GetFoundUsers(string searchName)
		{
			var response = await _client.GetAsync($"api/users?searchName={searchName}");
			return await response.ReadContentAs<List<UserToReturn>>();
		}

		public async Task AddPostLike(string userId, PostLike postLike)
		{
			var response = await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postLike.PostId}/likes", postLike);
		}
		public async Task DeletePostLike(string postId, string fromWho, string userId)
		{
			var response = await _client.DeleteFromApi($"api/users/{userId}/posts/{postId}/likes/{fromWho}");
		}
		public async Task AddPostComment(string userId, PostComment postComment)
		{
			var response = await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postComment.PostId}/comments", postComment);
		}
		public async Task DeletePostComment(string postId, string commentId, string userId)
		{
			var response = await _client.DeleteAsync($"api/users/{userId}/posts/{postId}/comments/{commentId}");
		}
	}
}
