using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Helpers;
using SemesterProject.ApiData.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Services
{
	public interface IMyFaceApiService
	{
		Task<HttpResponseMessage> AddFriend(string userId, RelationToAdd relationForAdd);
		Task<HttpResponseMessage> AddMessage(string userId, MessageToAdd messageForAdd);
		Task<HttpResponseMessage> AddNotification(NotificationToAdd notificationForAdd);
		Task<HttpResponseMessage> AddPost(string userId, PostToAdd postForAdd);
		Task<HttpResponseMessage> AddPostComment(string userId, PostComment postComment);
		Task<HttpResponseMessage> AddPostLike(string userId, PostLike postLike);
		Task<HttpResponseMessage> AddUserIfNotExist(ClaimsPrincipal userPrincipal);
		Task<bool> CheckIfAreFriends(Guid userId, Guid friendId);
		Task<HttpResponseMessage> DeleteFriend(string userId, string friendId);
		Task<HttpResponseMessage> DeletePost(string userId, string postId);
		Task<HttpResponseMessage> DeletePostComment(string postId, string commentId, string userId);
		Task<HttpResponseMessage> DeletePostLike(string postId, string fromWho, string userId);
		Task<HttpResponseMessage> DeleteNotification(string userId, string notificationId);
		Task<List<BasicUserData>> GetFoundUsers(string searchName);
		Task<List<BasicUserData>> GetFriends(string userId);
		Task<IEnumerable<Message>> GetMessages(string userId);
		Task<PagedList<Message>> GetMessagesWith(string userId, string friendId, PaginationParams paginationParams);
		Task<Notification> GetNotification(Guid userId, Guid eventId, Guid friendId);
		Task<IEnumerable<NotificationWithBasicFromWhoData>> GetNotifications(string userId);
		Task<Post> GetPost(string userId, string postId);
		Task<List<Post>> GetPosts(string userId);
		Task<UserToReturnWithCounters> GetUser(string userId);
		Task<HttpResponseMessage> MarkNotificationAsSeen(string userId, Guid notificationId);
		Task<HttpResponseMessage> UpdatePost(string userId, string postId, PostToUpdate postToUpdate);
	}
}