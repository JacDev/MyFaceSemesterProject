using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IPostApiAccess _postApiAccess;
        private readonly INotificationApiAccess _notificationApiAccess;
        private readonly IUserApiAccess _userApiAccess;
        private readonly ILogger<ProfileController> _logger;
        private readonly string _userId;
        public ProfileController(IHttpContextAccessor httpContextAccessor,
            IPostApiAccess postApiAccess, 
            INotificationApiAccess notificationApiAccess,
            IUserApiAccess userApiAccess,
            ILogger<ProfileController> logger)
        {
            _postApiAccess = postApiAccess;
            _notificationApiAccess = notificationApiAccess;
            _userApiAccess = userApiAccess;
            _logger = logger;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet]
        public async Task<ActionResult<UserPostsWithPostToAdd>> Index()
        {
            try
            {
                List<Post> posts = await _postApiAccess.GetPosts(_userId);
                posts.Reverse();

                UserPostsWithPostToAdd userToView = new UserPostsWithPostToAdd { Posts = posts, NewPost = new PostWithImageToAdd() };
                ViewData["userId"] = _userId.ToString();
                return View(userToView);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading user: {_userId} profile");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<NotificationWithBasicFromWhoData>>> Notifications()
        {
            try
            {
                IEnumerable<NotificationWithBasicFromWhoData> notifications = (await _notificationApiAccess.GetNotifications(_userId));
                foreach (var notification in notifications)
                {
                    await _notificationApiAccess.MarkNotificationAsSeen(_userId, notification.Notification.Id);
                }
                return View(notifications.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading user: {_userId} notifications");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}