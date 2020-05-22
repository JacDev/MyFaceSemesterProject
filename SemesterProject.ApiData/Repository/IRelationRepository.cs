using SemesterProject.ApiData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public interface IRelationRepository
	{
		List<Relation> GetUserRelations(Guid userId);
		Task AddRelationAsync(Guid userId, Guid friendId);
		Task DeleteRelationAsync(Guid userId, Guid friendId);
		bool CheckIfFriends(Guid firstUser, Guid secondUser);
	}
}
