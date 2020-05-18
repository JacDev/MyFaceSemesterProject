using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IMyFaceApiService _myFaceApiService;
        private readonly string _userId;
        public PostController(IMyFaceApiService myFaceApiService, IHttpContextAccessor httpContextAccessor)
        {
            _myFaceApiService = myFaceApiService;
            _myFaceApiService.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost([FromForm] UserPostsWithPostToAdd post)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (post.NewPost != null)
            {
                post.NewPost.WhenAdded = DateTime.Now;
                await _myFaceApiService.AddPost(userId, post.NewPost);
            }

            return RedirectToAction(nameof(Index), "Profile");
        }
        public async Task<IActionResult> DeletePost(string id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (!string.IsNullOrWhiteSpace(id))
            {
                await _myFaceApiService.DeletePost(userId, id);
            }
            return RedirectToAction(nameof(Index), "Profile");
        }

        [HttpGet("{userId}/{postId}")]
        public async Task<IActionResult> ShowComments(string userId, string postId)
        {
            if(string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(postId))
            {
                return NotFound();
            }
             var post = await _myFaceApiService.GetPost(userId, postId);
            if(post == null)
            {
                return NotFound();
            }
            
            ViewData["currentUserProfileId"] = userId.ToString();
            ViewData["loggedUserId"] = _userId.ToString();

            List<UserToReturnWithCounters> user = new List<UserToReturnWithCounters>();
            foreach(var userPost in post.PostComments)
            {
                user.Add(await _myFaceApiService.GetUser(userPost.FromWho.ToString()));
            }
            PostWithCommentToAdd postToReturn = new PostWithCommentToAdd
            {
                Post = post,
                Text = string.Empty,
                Users = user
            };

            return View(postToReturn);
        }
        [HttpPost("{userId}/{postId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(string userId, string postId, [FromForm] PostWithCommentToAdd postComment)
        {
            if (string.IsNullOrWhiteSpace(userId) || postComment==null)
            {
                return NotFound();
            }
            PostComment postCommentToAdd = new PostComment
            {
                WhenAdded = DateTime.Now,
                FromWho = Guid.Parse(_userId),
                PostId = Guid.Parse(postId),
                Text = postComment.Text
            };
            await _myFaceApiService.AddPostComment(userId, postCommentToAdd);
            return RedirectToAction(nameof(ShowComments), new { userId, postId });
        }
        [HttpPost("{userId}/{postId}/{commentId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(string userId, string postId, string commentId)
        {
            await _myFaceApiService.DeletePostComment(postId, commentId, userId);
            return RedirectToAction(nameof(ShowComments), new { userId, postId });
        }
        [HttpGet("EditPost/{userId}/{postId}")]
        public async Task<IActionResult> EditPost(string userId, string postId)
        {
            if (string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(userId))
            {
                return NotFound();
            }
            var post = await _myFaceApiService.GetPost(userId, postId);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
        [HttpPost("EditPost/{userId}/{postId}")]
        public async Task<IActionResult> EditPost(Post post)
        {
            if (post == null)
            {
                return NotFound();
            }
            await _myFaceApiService.UpdatePost(post.UserId.ToString(), post.Id.ToString(), new PostToUpdate
            {
                ImageFullPath = post.ImageFullPath,
                ImagePath = post.ImagePath,
                Text = post.Text
            });
            return RedirectToAction(nameof(Index), "Profile");
        }
    }
}