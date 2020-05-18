﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IMyFaceApiService _myFaceApiService;
        private readonly string _userId;
        public ProfileController(IMyFaceApiService myFaceApiService, IHttpContextAccessor httpContextAccessor)
        {
            _myFaceApiService = myFaceApiService;
            _myFaceApiService.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _myFaceApiService.GetPosts(_userId);
            posts.Reverse();

            ViewData["userId"] = _userId.ToString();
            UserPostsWithPostToAdd userToView = new UserPostsWithPostToAdd { Posts = posts, NewPost = new PostToAdd() };
            return View(userToView);
        }

        [HttpGet]
        public async Task<IActionResult> Notifications()
        {
            IEnumerable<NotificationWithBasicFromWhoData> notifications = (await _myFaceApiService.GetNotifications(_userId)).Where(n=>n.Notification.WasSeen==false);

            return View(notifications);
        }
        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}