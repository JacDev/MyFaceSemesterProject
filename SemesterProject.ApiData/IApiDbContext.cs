using Microsoft.EntityFrameworkCore;
using SemesterProject.ApiData.Entities;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.AppDbContext
{
	public interface IApiDbContext
	{
		DbSet<User> Users { get; set; }
		DbSet<Relation> Relations { get; set; }
		DbSet<Post> Posts { get; set; }
		DbSet<Message> Messages { get; set; }
		DbSet<Notification> Notifications { get; set; }
		DbSet<PostLike> PostLikes { get; set; }
		DbSet<PostComment> PostComments { get; set; }
		DbSet<Conversation> Conversations { get; set; }
		Task<int> SaveAsync();
	}
}
