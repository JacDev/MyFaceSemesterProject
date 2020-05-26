using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> logger;
        private readonly IPostApiAccess _postApiAccess;
        private readonly INotificationApiAccess _notificationApiAccess;
        private readonly IUserApiAccess _userApiAccess;
        private readonly string _userId;
        public ProfileController(IHttpContextAccessor httpContextAccessor,
            ILogger<ProfileController> logger,
            IPostApiAccess postApiAccess, 
            INotificationApiAccess notificationApiAccess,
            IUserApiAccess userApiAccess)
        {
            this.logger = logger;
            _postApiAccess = postApiAccess;
            _notificationApiAccess = notificationApiAccess;
            _userApiAccess = userApiAccess;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _postApiAccess.GetPosts(_userId);
            posts.Reverse();

            ViewData["userId"] = _userId.ToString();
            UserPostsWithPostToAdd userToView = new UserPostsWithPostToAdd { Posts = posts, NewPost = new PostWithImageToAdd() };
            return View(userToView);
        }

        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            IEnumerable<NotificationWithBasicFromWhoData> notifications = (await _notificationApiAccess.GetNotifications(_userId));
            foreach(var notification in notifications)
            {
                await _notificationApiAccess.MarkNotificationAsSeen(_userId, notification.Notification.Id);
            }
            return View(notifications);
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}