﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IPostApiAccess _postApiAccess;
        private readonly INotificationApiAccess _notificationApiAccess;
        private readonly IUserApiAccess _userApiAccess;
        private readonly IFriendApiAccess _friendApiAccess;
        private readonly string _userId;
        public FriendsController(IHttpContextAccessor httpContextAccessor,
            IPostApiAccess postApiAccess, 
            INotificationApiAccess notificationApiAccess,
            IUserApiAccess userApiAccess,
            IFriendApiAccess friendApiAccess)
        {
            _postApiAccess = postApiAccess;
            _notificationApiAccess = notificationApiAccess;
            _userApiAccess = userApiAccess;
            _friendApiAccess = friendApiAccess;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
        public async Task<IActionResult> ShowFriends()
        {
            var friends = await _friendApiAccess.GetFriends(_userId);

            return View(friends);
        }
        [HttpGet]
        public async Task<IActionResult> FindFriends()
        {
            var foundUsers = await _userApiAccess.GetFoundUsers("");
            ViewData["userId"] = _userId.ToString();
            return View(foundUsers);
        }

        public async Task<IActionResult> AddFriend(Guid userId)
        {
            var x = await _friendApiAccess.CheckIfAreFriends(Guid.Parse(_userId), userId);
            if (!x)
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
        public async Task<IActionResult> AcceptFriendRequiest(Guid notificationId, Guid friendId)
        {
            if (notificationId == Guid.Empty)
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
        public async Task<IActionResult> RejectFriendRequiest(Guid notificationId, Guid userId)
        {
            if (notificationId == Guid.Empty)
            {
                return NotFound();
            }

            await _notificationApiAccess.DeleteNotification(userId.ToString(), notificationId.ToString());

            return RedirectToAction("Notifications", "Profile");
        }
        public async Task<IActionResult> Deletefriend(Guid friendId)
        {
            await _friendApiAccess.DeleteFriend(_userId, friendId.ToString());
            return RedirectToAction(nameof(ShowFriends));
        }
        [HttpGet]
        public async Task<IActionResult> ViewProfile(Guid friendId)
        {
            var posts = await _postApiAccess.GetPosts(friendId.ToString());
            var user = await _userApiAccess.GetUser(friendId.ToString());
            posts.Reverse();

            UserWithPost userToView = new UserWithPost { Posts = posts, user = user };
            ViewData["friendId"] = friendId;
            ViewData["userId"] = _userId.ToString();
            return View(userToView);
        }
    }
}