using SemesterProject.ApiData.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public interface IMessageRepository
	{
		IQueryable<Message> GetUserMessagesWith(Guid userId, Guid friendId);
		Task AddMessageAsync(Message message);
		Task DeleteMessageAsync(Guid messageid);
		Message GetMessage(Guid messageId);
	}
}
