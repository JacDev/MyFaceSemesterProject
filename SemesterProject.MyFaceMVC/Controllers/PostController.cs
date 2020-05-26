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
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.FilesManager;
using SemesterProject.MyFaceMVC.Services;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IUserApiAccess _userApiAccess;
        private readonly IImagesManager _imagesManager;
        private readonly IPostApiAccess _postApiAccess;
        private readonly string _userId;
        public PostController(IUserApiAccess userApiAccess, 
            IHttpContextAccessor httpContextAccessor, 
            IImagesManager imagesManager,
            IPostApiAccess postApiAccess)
        {
            _userApiAccess = userApiAccess;
            _imagesManager = imagesManager;
            _postApiAccess = postApiAccess;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost([FromForm] UserPostsWithPostToAdd post)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (post.NewPost != null)
            {
                string imagePath = null, fullImagePath = null;
                if (post.NewPost.Picture != null)
                {
                    (imagePath, fullImagePath) = await _imagesManager.SaveImage(post.NewPost.Picture);
                }

                await _postApiAccess.AddPost(userId, new PostToAdd
                {
                  ImageFullPath = fullImagePath,
                  Text = post.NewPost.Text,
                  WhenAdded = DateTime.Now,
                  ImagePath = imagePath
                });
            }

            return RedirectToAction(nameof(Index), "Profile");
        }
        public async Task<IActionResult> DeletePost(string id)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (!string.IsNullOrWhiteSpace(id))
            {
                await _postApiAccess.DeletePost(userId, id);
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
             var post = await _postApiAccess.GetPost(userId, postId);
            if(post == null)
            {
                return NotFound();
            }
            
            ViewData["currentUserProfileId"] = userId.ToString();
            ViewData["loggedUserId"] = _userId.ToString();

            List<UserToReturnWithCounters> user = new List<UserToReturnWithCounters>();
            foreach(var userPost in post.PostComments)
            {
                user.Add(await _userApiAccess.GetUser(userPost.FromWho.ToString()));
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
            await _postApiAccess.AddPostComment(userId, postCommentToAdd);
            return RedirectToAction(nameof(ShowComments), new { userId, postId });
        }
        [HttpPost("{userId}/{postId}/{commentId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(string userId, string postId, string commentId)
        {
            await _postApiAccess.DeletePostComment(postId, commentId, userId);
            return RedirectToAction(nameof(ShowComments), new { userId, postId });
        }
        [HttpGet("EditPost/{userId}/{postId}")]
        public async Task<IActionResult> EditPost(string userId, string postId)
        {
            if (string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(userId))
            {
                return NotFound();
            }
            var post = await _postApiAccess.GetPost(userId, postId);
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
            await _postApiAccess.UpdatePost(post.UserId.ToString(), post.Id.ToString(), new PostToUpdate
            {
                ImageFullPath = post.ImageFullPath,
                ImagePath = post.ImagePath,
                Text = post.Text
            });
            return RedirectToAction(nameof(Index), "Profile");
        }
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_imagesManager.ImageStream(image), $"image/{mime}");
        }
    }
}