using SemesterProject.ApiData.AppDbContext;
using SemesterProject.ApiData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public class MessageRepository : IMessageRepository
	{
		private readonly IApiDbContext _appDbContext;
		public MessageRepository(IApiDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		public async Task AddMessageAsync(Message message)
		{
			if (message == null)
			{
				throw new ArgumentNullException(nameof(message));
			}

			if (message.FromWho == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(message.FromWho));
			}

			await _appDbContext.Messages.AddAsync(message);
			await _appDbContext.SaveAsync();
		}

		public async Task DeleteMessageAsync(Guid messageid)
		{
			if (messageid == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(messageid));
			}

			var messageToDelete = _appDbContext.Messages.FirstOrDefault(
				s => s.Id == messageid);

			if (messageToDelete == null)
			{
				throw new ArgumentNullException(nameof(messageToDelete));
			}
			_appDbContext.Messages.Remove(messageToDelete);
			await _appDbContext.SaveAsync();
		}

		public Message GetMessage(Guid messageId)
		{
			if (messageId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(messageId));
			}

			var messageToReturn = _appDbContext.Messages.FirstOrDefault(m => m.Id == messageId);

			if (messageToReturn == null)
			{
				throw new ArgumentNullException(nameof(messageToReturn));
			}
			return messageToReturn;
		}
		public IEnumerable<Message> GetLastMessages(Guid userId)
		{
			var lastMessages = _appDbContext.Messages.Where(x => x.ToWho == userId || x.FromWho == userId);







			return lastMessages;
		}

		public IQueryable<Message> GetUserMessagesWith(Guid userId, Guid friendId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}
			if (friendId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(friendId));
			}

			//null if no messages
			var messagesToReturn = _appDbContext.Messages.Where(
				m => m.FromWho == userId && m.ToWho == friendId
				|| m.FromWho == friendId && m.ToWho == userId);

			return messagesToReturn;
		}
	}
}
