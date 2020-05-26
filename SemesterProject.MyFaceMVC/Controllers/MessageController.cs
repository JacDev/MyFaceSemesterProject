using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Helpers;
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.Services;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IUserApiAccess _userApiAccess;
        private readonly IMessageApiAccess _messageApiService;
        private readonly string _userId;
        public MessageController(IUserApiAccess userApiAccess, IHttpContextAccessor httpContextAccessor, IMessageApiAccess messageApiService)
        {
            _userApiAccess = userApiAccess;
            _messageApiService = messageApiService;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public async Task<IActionResult> Index(string friendId)
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
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        public async Task<IActionResult> Messages()
        {
            var messages = await _messageApiService.GetMessages(_userId);

            List<MessagesWithUserData> messagesToReturn = new List<MessagesWithUserData>();

            foreach(var message in messages)
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
        public IActionResult JsLogin()
        {
            return View();
        }
    }
}