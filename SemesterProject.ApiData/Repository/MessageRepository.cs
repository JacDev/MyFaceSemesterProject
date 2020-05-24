using SemesterProject.ApiData.AppDbContext;
using SemesterProject.ApiData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SemesterProject.ApiData.Helpers;

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
			var conversation = _appDbContext.Conversations.FirstOrDefault(x => x.FirstUser == message.ToWho && x.SecondUser == message.FromWho
			|| x.FirstUser == message.FromWho && x.SecondUser == message.ToWho);

			if (conversation == null)
			{
				conversation = new Conversation
				{
					FirstUser = message.ToWho,
					SecondUser = message.FromWho
				};
				_appDbContext.Conversations.Add(conversation);
				
			}
			message.ConversationId = conversation.Id;
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
			var userConversations = _appDbContext.Conversations.Where(x => x.FirstUser == userId
			|| x.SecondUser == userId).Select(x=>x.Messages);

			List<Message> messagesToReturn = new List<Message>();
			foreach(var conv in userConversations)
			{
				messagesToReturn.AddRange(conv.OrderBy(x => x.When).Take(1));
			}


			return messagesToReturn;
		}

		public PagedList<Message> GetUserMessagesWith(Guid userId, Guid friendId, PaginationParams paginationParams)
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
			Conversation conversation = _appDbContext.Conversations.Include(nameof(_appDbContext.Messages)).FirstOrDefault(
				m => m.FirstUser == userId && m.SecondUser == friendId
				|| m.FirstUser == friendId && m.SecondUser == userId);
			if (conversation == null)
			{
				conversation = new Conversation
				{
					FirstUser = userId,
					SecondUser = friendId,
					Messages = new List<Message>()
				};
				_appDbContext.Conversations.Add(conversation);
			}

			var collection = conversation.Messages.OrderByDescending(x=>x.When).ToList();
			if (collection == null)
			{

				return PagedList<Message>.Create(null, 0, 0, 0);
			}
			return PagedList<Message>.Create(collection,
			   paginationParams.PageNumber,
			   paginationParams.PageSize,
			   paginationParams.Skip);
		}
	}
}
