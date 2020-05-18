using Microsoft.EntityFrameworkCore;
using SemesterProject.ApiData.AppDbContext;
using SemesterProject.ApiData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly IApiDbContext _appDbContext;
		public UserRepository(IApiDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		public async Task<User> GetUserAsync(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			var user = await _appDbContext.Users.Include("Posts").Include("Notifications").SingleOrDefaultAsync(x => x.Id == userId);
			user.Relations = _appDbContext.Relations.Where(r => r.FriendId == userId || r.UserId == userId).ToList(); ;
			return user;
		}
		public IQueryable<User> GetUsers()
		{
			return _appDbContext.Users;
		}
		public IEnumerable<User> GetUsers(IEnumerable<Guid> usersId)
		{
			if (usersId == null)
			{
				throw new ArgumentNullException(nameof(usersId));
			}
			return _appDbContext.Users.Where(a => usersId.Contains(a.Id))
				.OrderBy(a => a.FirstName)
				.OrderBy(a => a.LastName);
		}
		public async Task AddUserAcync(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			await _appDbContext.Users.AddAsync(user);
			await _appDbContext.SaveAsync();
		}
		public async Task UpdateUserAsync(User user)
		{
			await _appDbContext.SaveAsync();
		}
		public async Task DeleteUserAsync(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			_appDbContext.Users.Remove(user);
			await _appDbContext.SaveAsync();
		}

		public bool CheckIfUserExists(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}

			return _appDbContext.Users.Any(a => a.Id == userId);
		}
		public IEnumerable<User> GetUsers(string searchString)
		{
			return _appDbContext.Users.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));			
		}
	}
}
