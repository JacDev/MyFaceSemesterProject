using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Entities;
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

        [HttpGet("{friendId}")]
        public ActionResult<IEnumerable<Message>> GetMessages(Guid userId, string friendId)
        {
            if (userId == Guid.Empty || string.IsNullOrEmpty(friendId))
            {
                return NotFound();
            }

            if (!_userRepository.CheckIfUserExists(userId) || !_userRepository.CheckIfUserExists(Guid.Parse(friendId)))
            {
                return NotFound();
            }

            List<Message> userMessages = _messageRepository.GetUserMessagesWith(userId, Guid.Parse(friendId)).Take(10).ToList(); ;

            return Ok(userMessages);
        }
        [HttpPost]
        public async Task<ActionResult> AddMessage(Guid userId, [FromBody] MessageToAdd message)
        {
            if (message == null && userId!=Guid.Empty)
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

    }
}