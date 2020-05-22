using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SemesterProject.ApiData.Entities;

namespace SemesterProject.ApiData.AppDbContext
{
	public class ApiDbContext : DbContext, IApiDbContext
	{
		public ApiDbContext(DbContextOptions options) : base(options)
		{
		}
		public DbSet<User> Users { get; set; }
		public DbSet<Relation> Relations { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<PostLike> PostLikes { get; set; }
		public DbSet<PostComment> PostComments { get; set; }
		public DbSet<Conversation> Conversations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Relation>().HasKey(s => new { s.UserId, s.FriendId });
		}
		public async Task<int> SaveAsync()
		{
			try
			{
				return await SaveChangesAsync();
			}
			catch
			{
				throw;
			}
		}
	}
}
