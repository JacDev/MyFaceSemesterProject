using SemesterProject.MyFaceMVC.Data;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Repository
{
	public interface IOnlineUsersRepository
	{
		bool IsUserOnline(string userId);
		Task AddOnlineUser(OnlineUserModel onlineUserModel);
		OnlineUserModel GetOnlineUser(string userId);

		Task SetUserChatConnectionId(string userId, string connectionId);
		Task SetUserNotificationConnectionId(string userId, string connectionId);

		string GetUserChatConnetionId(string userId);
		string GetUserNotificationConnetionId(string userId);

		Task ClearChatConnectionIdOrRemoveUser(string userId);
		Task ClearNotificationConnectionIdOrRemoveUser(string userId);
	}
}