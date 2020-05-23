using SemesterProject.ApiData.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using SemesterProject.ApiData.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;
using SemesterProject.ApiData.Helpers;
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
		public async Task<List<BasicUserData>> GetFriends(string userId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetAsync($"api/users/{userId}/friends");
				return await response.ReadContentAs<List<BasicUserData>>();
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> AddFriend(string userId, RelationToAdd relationForAdd)
		{
			try
			{
				return await _client.PostToApiAsJsonAsync($"api/users/{userId}/friends", relationForAdd);
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> DeleteFriend(string userId, string friendId)
		{
			try
			{
				return await _client.DeleteFromApi($"api/users/{userId}/friends/{friendId}");
			}
			catch
			{
				throw;
			}
		}
		public async Task<bool> CheckIfAreFriends(Guid userId, Guid friendId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}/friends/{friendId}");
				return await response.ReadContentAs<bool>();
			}
			catch
			{
				throw;
			}
	}

		public async Task<Post> GetPost(string userId, string postId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}/posts/{postId}");
				return await response.ReadContentAs<Post>();
			}
			catch
			{
				throw;
			}
		}
		public async Task<List<Post>> GetPosts(string userId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}/posts");
				return await response.ReadContentAs<List<Post>>();
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> AddPost(string userId, PostToAdd postForAdd)
		{
			try
			{
				return await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts", postForAdd);
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> UpdatePost(string userId, string postId, PostToUpdate postToUpdate)
		{
			try
			{
				return await _client.PatchToApiAsJsonAsync($"api/users/{userId}/posts/{postId}", postToUpdate);
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> DeletePost(string userId, string postId)
		{
			try
			{
				return await _client.DeleteAsync($"api/users/{userId}/posts/{postId}");
			}
			catch
			{
				throw;
			}
		}

		public async Task<HttpResponseMessage> AddPostLike(string userId, PostLike postLike)
		{
			try
			{
				return await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postLike.PostId}/likes", postLike);
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> DeletePostLike(string postId, string fromWho, string userId)
		{
			try
			{
				return await _client.DeleteFromApi($"api/users/{userId}/posts/{postId}/likes/{fromWho}");
			}
			catch
			{
				throw;
			}
		}

		public async Task<HttpResponseMessage> AddPostComment(string userId, PostComment postComment)
		{
			try
			{
				return await _client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postComment.PostId}/comments", postComment);
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> DeletePostComment(string postId, string commentId, string userId)
		{
			try
			{
				return await _client.DeleteAsync($"api/users/{userId}/posts/{postId}/comments/{commentId}");
			}
			catch
			{
				throw;
			}
		}

		public async Task<IEnumerable<NotificationWithBasicFromWhoData>> GetNotifications(string userId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}/notifications");
				return await response.ReadContentAs<List<NotificationWithBasicFromWhoData>>();
			}
			catch
			{
				throw;
			}
		}
		public async Task<Notification> GetNotification(Guid userId, Guid eventId, Guid friendId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}/notifications/{eventId}/{friendId}");
				return await response.ReadContentAs<Notification>();
			}
			catch
			{
				throw;
			}
		}
		
		public async Task<HttpResponseMessage> AddNotification(NotificationToAdd notificationForAdd)
		{
			try
			{
				return await _client.PostToApiAsJsonAsync($"api/users/{notificationForAdd.UserId}/notifications", notificationForAdd);
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> MarkNotificationAsSeen(string userId, Guid notificationId)
		{
			try
			{
				return await _client.PatchToApiAsJsonAsync($"api/users/{userId}/notifications/{notificationId}", new Notification());
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> DeleteNotification(string userId, string notificationId)
		{
			try
			{
				return await _client.DeleteFromApi($"api/users/{userId}/notifications/{notificationId}");
			}
			catch
			{
				throw;
			}
		}
		

		public async Task<PagedList<Message>> GetMessagesWith(string userId, string friendId, PaginationParams paginationParams)
		{
			try
			{
				var urlParams = new QueryBuilder
				{
					{ nameof(paginationParams.PageNumber), paginationParams.PageNumber.ToString() },
					{ nameof(paginationParams.PageSize), paginationParams.PageSize.ToString() }
				};

				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}/messages/{friendId}/{urlParams}");
				return await response.ReadContentAs<PagedList<Message>>();
			}
			catch
			{
				throw;
			}
		}
		public async Task<IEnumerable<Message>> GetMessages(string userId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}/messages");
				return await response.ReadContentAs<List<Message>>();
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> AddMessage(string userId, MessageToAdd messageForAdd)
		{
			try
			{
				return await _client.PostToApiAsJsonAsync($"api/users/{userId}/messages", messageForAdd);
			}
			catch
			{
				throw;
			}
		}

		public async Task<UserToReturnWithCounters> GetUser(string userId)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users/{userId}");
				return await response.ReadContentAs<UserToReturnWithCounters>();
			}
			catch
			{
				throw;
			}
		}
		public async Task<HttpResponseMessage> AddUserIfNotExist(ClaimsPrincipal userPrincipal)
		{
			try
			{
				BasicUserData userToSend = new BasicUserData
				{
					Id = Guid.Parse(userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
					FirstName = userPrincipal.Claims.FirstOrDefault(x => x.Type == "FirstName").Value,
					LastName = userPrincipal.Claims.FirstOrDefault(x => x.Type == "LastName").Value,
				};
				return await _client.PostToApiAsJsonAsync("api/users", userToSend);
			}
			catch
			{
				throw;
			}
		}
		public async Task<List<BasicUserData>> GetFoundUsers(string searchName)
		{
			try
			{
				HttpResponseMessage response = await _client.GetFromApiAsync($"api/users?searchName={searchName}");
				return await response.ReadContentAs<List<BasicUserData>>();
			}
			catch
			{
				throw;
			}
		}
	}
}