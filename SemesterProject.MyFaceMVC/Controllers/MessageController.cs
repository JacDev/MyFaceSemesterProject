using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Helpers;
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    public class MessageController : Controller
    {
        private readonly IUserApiAccess _userApiAccess;
        private readonly IMessageApiAccess _messageApiService;
        private readonly ILogger<MessageController> _logger;
        private readonly string _userId;
        public MessageController(IUserApiAccess userApiAccess,
            IHttpContextAccessor httpContextAccessor,
            IMessageApiAccess messageApiService,
            ILogger<MessageController> logger)
        {
            _userApiAccess = userApiAccess;
            _messageApiService = messageApiService;
            _logger = logger;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
        [HttpGet]
        public async Task<ActionResult<PagedList<Message>>> Index(string friendId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(friendId))
                {
                    PagedList<Message> messages = await _messageApiService.GetMessagesWith(_userId, friendId, new PaginationParams { PageNumber = 0, PageSize = 10 });
                    var friend = await _userApiAccess.GetUser(friendId);
                    ViewData["friendId"] = friendId;
                    ViewData["userId"] = _userId.ToString();
                    if (friend != null)
                    {
                        ViewData["friendFirstName"] = friend.FirstName;
                        ViewData["friendLastName"] = friend.LastName;
                    }
                    return View(messages);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading messages with: {friendId} by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<MessagesWithUserData>>> Messages()
        {
            try
            {
                IEnumerable<Message> messages = await _messageApiService.GetMessages(_userId);
                List<MessagesWithUserData> messagesToReturn = new List<MessagesWithUserData>();

                foreach (Message message in messages)
                {
                    messagesToReturn.Add(new MessagesWithUserData
                    {
                        Message = message,
                        User = await _userApiAccess.GetUser(message.FromWho.ToString())
                    });
                }
                ViewData["userId"] = _userId.ToString();
                return View(messagesToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading user: {_userId} messages");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        public IActionResult JsLogin()
        {
            return View();
        }
    }
}