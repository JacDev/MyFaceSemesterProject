using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Models;
using SemesterProject.MyFaceMVC.Services;

namespace SemesterProject.MyFaceMVC.Controllers
{
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly IMyFaceApiService _myFaceApiService;
        private readonly string _userId;
        public FriendsController(IMyFaceApiService myFaceApiService, IHttpContextAccessor httpContextAccessor)
        {
            _myFaceApiService = myFaceApiService;
            _myFaceApiService.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }
        public async Task<IActionResult> ShowFriends()
        {
            var friends = await _myFaceApiService.GetFriends(_userId);

            return View(friends);
        }
        [HttpGet]
        public IActionResult FindFriends()
        {
            //var foundUsers = _myFaceApiServic

            return View(new List<UserToReturn>());
        }
        [HttpPost]
        public async Task<IActionResult> FindFriends(string searchName)
        {
            var foundUsers = await _myFaceApiService.GetFoundUsers(searchName);

            return View(foundUsers);
        }
        public async Task<IActionResult> AddFriend(Guid userId)
        {
            await _myFaceApiService.AddNotification(_userId, new NotificationToAdd
            {
                FromWho = Guid.Parse(_userId),
                UserId = userId,
                WasSeen = false
            });
            return RedirectToAction(nameof(FindFriends));
        }
        public async Task<IActionResult> AcceptFriendRequiest(Guid notificationId, Guid friendId)
        {
            if (notificationId == Guid.Empty)
            {
                return BadRequest();
            }

            await _myFaceApiService.MarkNotificationAsSeen(_userId, notificationId);
            await _myFaceApiService.AddFriendsRelation(_userId, new RelationToAdd
            {
                FriendId = friendId,
                SinceWhen = DateTime.Now
            });

            return RedirectToAction("Notifications", "Profile");
        }
        public async Task<IActionResult> Deletefriend(Guid friendId)
        {
            await _myFaceApiService.DeleteFriendsRelation(_userId, friendId.ToString());
            return RedirectToAction(nameof(ShowFriends));
        }
        [HttpGet]
        public async Task<IActionResult> ViewProfile(Guid friendId)
        {
            var posts = await _myFaceApiService.GetPosts(friendId.ToString());
            var user = await _myFaceApiService.GetUser(friendId.ToString());
            posts.Reverse();

            UserWithPost userToView = new UserWithPost { Posts = posts, user = user };
            ViewData["friendId"] = friendId;
            ViewData["userId"] = _userId.ToString();
            return View(userToView);
        }
        //public void AddComment(Guid postId)
        //{

        //}
        //public void UndoComment(Guid postId)
        //{

        //}
    }
}