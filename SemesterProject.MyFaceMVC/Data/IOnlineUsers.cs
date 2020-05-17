using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Data
{
	public interface IOnlineUsers
	{
		DbSet<OnlineUserModel> OnlineUsers { get; set; }
		Task<int> SaveAsync();
	}
}