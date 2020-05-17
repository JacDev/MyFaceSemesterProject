using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public interface IUserRepository
	{
		Task AddUserAcync(User user);
		Task DeleteUserAsync(User user);
		Task<User> GetUserAsync(Guid userId);
		IEnumerable<User> GetUsers(IEnumerable<Guid> usersId);
		IQueryable<User> GetUsers();
		Task UpdateUserAsync(User user);
		bool CheckIfUserExists(Guid userId);
		IEnumerable<User> GetUsers(string searchString);
	}
}
