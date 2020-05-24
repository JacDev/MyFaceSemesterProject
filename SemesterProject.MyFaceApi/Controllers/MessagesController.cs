using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Helpers;
using SemesterProject.ApiData.Models;
using SemesterProject.ApiData.Repository;

namespace SemesterProject.MyFaceApi.Controllers
{
	[Route("api/users/{userId}/messages")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		private readonly IMessageRepository _messageRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		public MessagesController(IMessageRepository messageRepository, IMapper mapper, IUserRepository userRepository)
		{
			_messageRepository = messageRepository ??
				throw new ArgumentNullException(nameof(messageRepository));
			_mapper = mapper ??
				throw new ArgumentNullException(nameof(mapper));
			_userRepository = userRepository ??
				throw new ArgumentNullException(nameof(userRepository));
		}
		public IActionResult GetMessagesOpitons()
		{
			Response.Headers.Add("Allow", "GET,OPTIONS,POST");
			return Ok();
		}

		[HttpGet]
		public ActionResult<IEnumerable<Message>> GetMessage(Guid userId)
		{
			if (userId == Guid.Empty || !_userRepository.CheckIfUserExists(userId))
			{
				return NotFound();
			}
			return Ok(_messageRepository.GetLastMessages(userId));
		}

		[HttpGet("{friendId}", Name = "GetMessages")]
		public ActionResult<PagedList<Message>> GetMessagesWith(Guid userId, string friendId, [FromQuery] PaginationParams paginationParams)
		{
			if (userId == Guid.Empty || string.IsNullOrEmpty(friendId))
			{
				return NotFound();
			}

			if (!_userRepository.CheckIfUserExists(userId) || !_userRepository.CheckIfUserExists(Guid.Parse(friendId)))
			{
				return NotFound();
			}

			PagedList<Message> userMessages = _messageRepository.GetUserMessagesWith(userId, Guid.Parse(friendId), paginationParams);
			if (userMessages != null)
			{
				var previousPageLink = userMessages.HasPrevious ?
					CreateAuthorsResourceUri(paginationParams, ResourceUriType.PreviousPage) : null;

				var nextPageLink = userMessages.HasNext ?
					CreateAuthorsResourceUri(paginationParams, ResourceUriType.NextPage) : null;

				userMessages.PreviousPageLink = previousPageLink;
				userMessages.NextPageLink = nextPageLink;

				var paginationMetadata = new
				{
					totalCount = userMessages.TotalCount,
					pageSize = userMessages.PageSize,
					currentPage = userMessages.CurrentPage,
					totalPages = userMessages.TotalPages,
					previousPageLink,
					nextPageLink

				};
				Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
			}

			return Ok(userMessages);
		}

		[HttpPost]
		public async Task<ActionResult> AddMessage(Guid userId, [FromBody] MessageToAdd message)
		{
			if (message == null && userId != Guid.Empty)
			{
				return NotFound();
			}
			if (!_userRepository.CheckIfUserExists(message.ToWho) || !_userRepository.CheckIfUserExists(userId))
			{
				return NotFound();
			}
			var messageToAdd = _mapper.Map<Message>(message);
			messageToAdd.FromWho = userId;
			await _messageRepository.AddMessageAsync(messageToAdd);
			return NoContent();
		}
		private string CreateAuthorsResourceUri(
			PaginationParams paginationParams,
			ResourceUriType type)
		{
			int pageNumber;
			switch (type)
			{
				case ResourceUriType.PreviousPage:
					pageNumber = paginationParams.PageNumber - 1;
					break;
				case ResourceUriType.NextPage:
					pageNumber = paginationParams.PageNumber + 1;
					break;
				default:
					pageNumber = paginationParams.PageNumber;
					break;
			}
			return Url.Link("GetMessages", new
			{
				pageNumber = pageNumber,
				pageSize = paginationParams.PageSize
			});
		}
		

	}
}