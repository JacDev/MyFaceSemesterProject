using SemesterProject.ApiData.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public interface IUserRepository
	{
		Task AddProfilePicture(Guid userId, string imagePath);
		Task AddUserAcync(User user);
		Task DeleteUserAsync(User user);
		Task<User> GetUserAsync(Guid userId);
		List<User> GetUsers(IEnumerable<string> usersId);
		List<User> GetUsers(IEnumerable<Guid> usersId);
		List<User> GetUsers();
		Task UpdateUserAsync(User user);
		bool CheckIfUserExists(Guid userId);
		List<User> GetUsers(string searchString);
	}
}
