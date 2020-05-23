using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public interface IMessageRepository
	{
		PagedList<Message> GetUserMessagesWith(Guid userId, Guid friendId, PaginationParams paginationParams);
		Task AddMessageAsync(Message message);
		Task DeleteMessageAsync(Guid messageid);
		Message GetMessage(Guid messageId);
		IEnumerable<Message> GetLastMessages(Guid userId);
	}
}
