using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Services
{
	public interface IMyFaceApiService
	{
		Task AddFriendsRelation(string userId, RelationToAdd relationForAdd);
		Task AddMessage(string userId, MessageToAdd messageForAdd);
		Task<IEnumerable<Message>> GetMessagesWith(string userId, string friendId);
		Task AddNotification(string userId, NotificationToAdd notificationForAdd);
		Task<IEnumerable<NotificationWithBasicFromWhoData>> GetNotifications(string userId);
		Task AddPost(string userId, PostToAdd postForAdd);
		Task<Post> GetPost(string userId, string postId);
		Task DeleteFriendsRelation(string userId, string friendId);
		Task<List<UserToReturnAsFriend>> GetFriends(string userId);
		Task<UserToReturn> GetUser(string userId);
		Task<List<Post>> GetPosts(string userId);
		Task DeletePost(string userId, string postId);
		Task MarkNotificationAsSeen(string userId, Guid notificationId);
		Task UpdatePost(string userId, string postId, PostToUpdate postForUpdate);
		Task AddUserIfNotExist(ClaimsPrincipal principal);
		Task<List<UserToReturn>> GetFoundUsers(string searchName);
		Task AddPostLike(string userId, PostLike post);
		Task DeletePostLike(string postId, string fromWho, string userId);
		Task AddPostComment(string userId, PostComment postComment);
		Task DeletePostComment(string postId, string commentId, string userId);
	}
}