using SemesterProject.MyFaceMVC.Data;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Repository
{
	public interface IOnlineUsersRepository
	{
		bool IsUserOnline(string userId);
		Task AddOnlineUser(OnlineUserModel onlineUserModel);
		OnlineUserModel GetOnlineUser(string userId);
		Task RemoveUser(OnlineUserModel user);
	}
}