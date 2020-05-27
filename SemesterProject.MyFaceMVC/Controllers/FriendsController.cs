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
    public class FriendsController : Controller
    {
        private readonly IPostApiAccess _postApiAccess;
        private readonly INotificationApiAccess _notificationApiAccess;
        private readonly IUserApiAccess _userApiAccess;
        private readonly IFriendApiAccess _friendApiAccess;
        private readonly ILogger<FriendsController> _logger;
        private readonly string _userId;
        public FriendsController(IHttpContextAccessor httpContextAccessor,
            IPostApiAccess postApiAccess,
            INotificationApiAccess notificationApiAccess,
            IUserApiAccess userApiAccess,
            IFriendApiAccess friendApiAccess,
            ILogger<FriendsController> logger)
        {
            _postApiAccess = postApiAccess;
            _notificationApiAccess = notificationApiAccess;
            _userApiAccess = userApiAccess;
            _friendApiAccess = friendApiAccess;
            _logger = logger;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
        [HttpGet]
        public async Task<ActionResult<List<BasicUserData>>> ShowFriends()
        {
            try
            {
                List<BasicUserData> friends = await _friendApiAccess.GetFriends(_userId);
                return View(friends);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading user {_userId} friends");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<BasicUserData>>> FindFriends()
        {
            try
            {
                List<BasicUserData> foundUsers = await _userApiAccess.GetFoundUsers("");
                ViewData["userId"] = _userId.ToString();
                return View(foundUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading user {_userId} friends");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddFriend(Guid userId)
        {
            try
            {
                bool areFriends = await _friendApiAccess.CheckIfAreFriends(Guid.Parse(_userId), userId);
                if (!areFriends)
                {
                    await _notificationApiAccess.AddNotification(new NotificationToAdd
                    {
                        FromWho = Guid.Parse(_userId),
                        UserId = userId,
                        WasSeen = false,
                        NotificationType = NotificationType.FriendRequiest
                    });
                }
                return RedirectToAction(nameof(FindFriends));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during adding friend requiest notification by user {_userId} to user {userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        public async Task<IActionResult> AcceptFriendRequiest(Guid notificationId, Guid friendId)
        {
            try
            {
                if (notificationId == Guid.Empty || friendId == Guid.Empty)
                {
                    return NotFound();
                }

                await _notificationApiAccess.MarkNotificationAsSeen(_userId, notificationId);
                await _friendApiAccess.AddFriend(_userId, new RelationToAdd
                {
                    FriendId = friendId,
                    SinceWhen = DateTime.Now
                });

                return RedirectToAction("Notifications", "Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during adding friend by user {_userId} to user {friendId}. Notification id: {notificationId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        public async Task<IActionResult> RejectFriendRequiest(Guid notificationId, Guid userId)
        {
            try
            {
                if (notificationId == Guid.Empty || userId == Guid.Empty)
                {
                    return NotFound();
                }

                await _notificationApiAccess.DeleteNotification(userId.ToString(), notificationId.ToString());
                return RedirectToAction("Notifications", "Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during removing friend requiest by user {_userId} to user {userId}. Notification id: {notificationId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        public async Task<IActionResult> Deletefriend(Guid friendId)
        {
            try
            {
                await _friendApiAccess.DeleteFriend(_userId, friendId.ToString());
                return RedirectToAction(nameof(ShowFriends));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during removing friend by user {_userId} to user {friendId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> ViewProfile(Guid friendId)
        {
            try
            {
                List<Post> posts = await _postApiAccess.GetPosts(friendId.ToString());
                UserToReturnWithCounters user = await _userApiAccess.GetUser(friendId.ToString());
                posts.Reverse();

                UserWithPost userToView = new UserWithPost { Posts = posts, user = user };
                //ViewData["friendId"] = friendId;
                ViewData["userId"] = _userId.ToString();
                return View(userToView);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading user profile: {friendId} by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
    }
}