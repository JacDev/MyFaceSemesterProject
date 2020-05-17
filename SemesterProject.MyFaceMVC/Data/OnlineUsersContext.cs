using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Data
{
	public class ChatOnlineUsersContext : DbContext, IOnlineUsers
	{
		public ChatOnlineUsersContext(DbContextOptions<ChatOnlineUsersContext> options) : base(options)
		{
		}
		public DbSet<OnlineUserModel> OnlineUsers { get; set; }

		public async Task<int> SaveAsync()
		{
			return await SaveChangesAsync();
		}
	}
}
