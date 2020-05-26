using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
		private readonly ILogger _logger;

		public UserRepository(IApiDbContext appDbContext,
			ILoggerFactory loggerFactory)
		{
			_appDbContext = appDbContext;
			_logger = loggerFactory.CreateLogger("UserDatabase");
		}
		public async Task<User> GetUserAsync(Guid userId)
		{
		
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			_logger.LogDebug($"Trying to get user {userId}...");
			try
			{
				User user = await _appDbContext.Users
					.Include("Posts")
					.Include("Notifications")
					.SingleOrDefaultAsync(x => x.Id == userId);

				if (user != null)
				{
					_logger.LogDebug($"User {user.FirstName} {user.LastName} found.");
					user.Relations = _appDbContext.Relations
					.Where(r => r.FriendId == userId || r.UserId == userId)
					.ToList();
				}
				else
				{
					_logger.LogDebug($"User {userId} not found.");
				}
				return user;
			}
			catch
			{
				throw;
			}
		}
		public List<User> GetUsers()
		{
				return _appDbContext.Users.ToList();
		}
		public List<User> GetUsers(IEnumerable<Guid> usersId)
		{		
			if (usersId == null)
			{
				throw new ArgumentNullException(nameof(usersId));
			}

			_logger.LogDebug("Trying to get users...");
			try
			{
				List<User> users = _appDbContext.Users.Where(a => usersId.Contains(a.Id))
					.OrderBy(a => a.FirstName)
					.OrderBy(a => a.LastName)
					.ToList();
				if (users == null)
				{
					_logger.LogDebug("No user was found.");
				}
				else
				{
					_logger.LogDebug("Some users found.");
				}
				return users;
			}
			catch
			{
				_logger.LogWarning("Something went wrong while searching users");
				throw;
			}
		}
		public List<User> GetUsers(IEnumerable<string> usersId)
		{
			if (usersId == null)
			{
				throw new ArgumentNullException(nameof(usersId));
			}
			var usersGuid = new List<Guid>();
			foreach (var id in usersId)
			{
				usersGuid.Add(Guid.Parse(id));
			}
			_logger.LogDebug("Trying to get users...");
			try
			{
				List<User> users = _appDbContext.Users.Where(a => usersGuid.Contains(a.Id))
					.OrderBy(a => a.FirstName)
					.OrderBy(a => a.LastName)
					.ToList();
				if (users == null)
				{
					_logger.LogDebug("No user was found.");
				}
				else
				{
					_logger.LogDebug("Some users found.");
				}
				return users;
			}
			catch
			{
				_logger.LogWarning("Something went wrong while searching users");
				throw;
			}
		}
		public async Task AddUserAcync(User user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			_logger.LogDebug("Trying to add user {user}.", user);
			try
			{
				await _appDbContext.Users.AddAsync(user);
				await _appDbContext.SaveAsync();
				_logger.LogDebug("User {user} has been addes.", user);
			}
			catch
			{
				_logger.LogWarning("Something went wrong while adding user: {user}", user);
				throw;
			}
		}
		public async Task UpdateUserAsync(User user)
		{
			_logger.LogDebug("Trying to update user {user}", user);
			try
			{
				await _appDbContext.SaveAsync();
				_logger.LogDebug("User {user} has been updated.", user);
			}
			catch
			{
				_logger.LogWarning("Something went wrong while updating user: {user}", user);
				throw;
			}
		}
		public async Task DeleteUserAsync(User user)
		{
			_logger.LogDebug("Trying to remove user {user}.", user);
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			try
			{
				_appDbContext.Users.Remove(user);
				await _appDbContext.SaveAsync();
				_logger.LogDebug("User {user} has been removed.", user);
			}
			catch
			{
				_logger.LogWarning("Something went wrong while removing user: {user}.", user);
				throw;
			}
		}

		public bool CheckIfUserExists(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			_logger.LogDebug($"Trying to check if exist user {userId}");
			try
			{
				bool wasFound = _appDbContext.Users.Any(a => a.Id == userId);
				if (wasFound)
				{
					_logger.LogDebug($"User {userId} exist.");
				}
				else
				{
					_logger.LogDebug($"User {userId} does not exist.");
				}
				return wasFound;
			}
			catch
			{
				_logger.LogWarning($"Something went wrong while searching user: {userId}.");
				throw;
			}
		}
		public List<User> GetUsers(string searchString)
		{
			try
			{
				return _appDbContext.Users
					.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString))
					.ToList();
			}
			catch
			{
				_logger.LogWarning($"Something went wrong while searching users containing: {searchString}.");
				throw;
			}
		}
	}
}
