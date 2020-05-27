using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public class PostApiAccess : IPostApiAccess
	{
		private readonly IMyFaceApiService _myFaceApiService;

		public PostApiAccess(IMyFaceApiService myFaceApiService)
		{
			_myFaceApiService = myFaceApiService;
		}
		public async Task<Post> GetPost(string userId, string postId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/posts/{postId}");
			return await response.ReadContentAs<Post>();
		}
		public async Task<List<Post>> GetPosts(string userId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/posts");
			return await response.ReadContentAs<List<Post>>();
		}
		public async Task<List<Post>> GetLatestPosts(string userId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/posts/latest");
			return await response.ReadContentAs<List<Post>>();
		}
		public async Task<HttpResponseMessage> AddPost(string userId, PostToAdd postForAdd)
		{
			return await _myFaceApiService.Client.PostToApiAsJsonAsync($"api/users/{userId}/posts", postForAdd);
		}
		public async Task<HttpResponseMessage> UpdatePost(string userId, string postId, PostToUpdate postToUpdate)
		{
			return await _myFaceApiService.Client.PatchToApiAsJsonAsync($"api/users/{userId}/posts/{postId}", postToUpdate);
		}
		public async Task<HttpResponseMessage> DeletePost(string userId, string postId)
		{
			return await _myFaceApiService.Client.DeleteAsync($"api/users/{userId}/posts/{postId}");
		}
		public async Task<HttpResponseMessage> AddPostLike(string userId, PostLike postLike)
		{
			return await _myFaceApiService.Client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postLike.PostId}/likes", postLike);
		}
		public async Task<HttpResponseMessage> DeletePostLike(string postId, string fromWho, string userId)
		{
			return await _myFaceApiService.Client.DeleteFromApi($"api/users/{userId}/posts/{postId}/likes/{fromWho}");
		}
		public async Task<HttpResponseMessage> AddPostComment(string userId, PostComment postComment)
		{
			return await _myFaceApiService.Client.PostToApiAsJsonAsync($"api/users/{userId}/posts/{postComment.PostId}/comments", postComment);
		}
		public async Task<HttpResponseMessage> DeletePostComment(string postId, string commentId, string userId)
		{
			return await _myFaceApiService.Client.DeleteAsync($"api/users/{userId}/posts/{postId}/comments/{commentId}");
		}
	}
}
