using SemesterProject.ApiData.AppDbContext;
using SemesterProject.ApiData.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public class RelationRepository : IRelationRepository
	{
		private readonly IApiDbContext _appDbContext;
		public RelationRepository(IApiDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		public async Task AddRelationAsync(Guid userId, Guid friendId)
		{
			if(userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			if (friendId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(friendId));
			}
			if(_appDbContext.Relations.Any(
				s => s.UserId == userId && s.FriendId == friendId
				|| s.UserId == friendId && s.FriendId == userId))
			{
				return;
			}
			await _appDbContext.Relations.AddAsync(new Relation
			{
				UserId = userId,
				FriendId = friendId,
				SinceWhen = DateTime.Now
			});
			await _appDbContext.SaveAsync();
		}
		public async Task DeleteRelationAsync(Guid userId, Guid friendId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			if (friendId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(friendId));
			}
			var relationToDelete =_appDbContext.Relations.FirstOrDefault(
				s => s.UserId == userId && s.FriendId == friendId
				|| s.UserId == friendId && s.FriendId == userId);
			if(relationToDelete == null)
			{
				throw new ArgumentNullException(nameof(relationToDelete));
			}
			_appDbContext.Relations.Remove(relationToDelete);
			await _appDbContext.SaveAsync();
		}		public IQueryable<Relation> GetUserRelations(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			return _appDbContext.Relations.Where(s => s.UserId == userId || s.FriendId == userId);
		}
	}
}
