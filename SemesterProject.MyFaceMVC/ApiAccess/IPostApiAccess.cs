using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public interface IPostApiAccess
	{
		Task<HttpResponseMessage> AddPost(string userId, PostToAdd postForAdd);
		Task<HttpResponseMessage> AddPostComment(string userId, PostComment postComment);
		Task<HttpResponseMessage> AddPostLike(string userId, PostLike postLike);
		Task<HttpResponseMessage> DeletePost(string userId, string postId);
		Task<HttpResponseMessage> DeletePostComment(string postId, string commentId, string userId);
		Task<HttpResponseMessage> DeletePostLike(string postId, string fromWho, string userId);
		Task<List<Post>> GetLatestPosts(string userId);
		Task<Post> GetPost(string userId, string postId);
		Task<List<Post>> GetPosts(string userId);
		Task<HttpResponseMessage> UpdatePost(string userId, string postId, PostToUpdate postToUpdate);
	}
}