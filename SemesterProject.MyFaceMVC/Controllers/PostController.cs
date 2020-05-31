using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.ApiAccess;
using SemesterProject.MyFaceMVC.FilesManager;
using SemesterProject.MyFaceMVC.ViewModels;

namespace SemesterProject.MyFaceMVC.Controllers
{
    public class PostController : Controller
    {
        private readonly IUserApiAccess _userApiAccess;
        private readonly IImagesManager _imagesManager;
        private readonly IPostApiAccess _postApiAccess;
        private readonly ILogger<PostController> _logger;
        private readonly IMapper _mapper;
        private readonly string _userId;
        public PostController(IUserApiAccess userApiAccess, 
            IHttpContextAccessor httpContextAccessor, 
            IImagesManager imagesManager,
            IPostApiAccess postApiAccess,
            ILogger<PostController> logger,
            IMapper mapper)
        {
            _userApiAccess = userApiAccess;
            _imagesManager = imagesManager;
            _postApiAccess = postApiAccess;
            _logger = logger;
            _mapper = mapper;
            _userApiAccess.AddUserIfNotExist(httpContextAccessor.HttpContext.User).GetAwaiter();
            _userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost([FromForm] UserPostsWithPostToAdd post)
        {
            try
            {
                if (post.NewPost != null)
                {
                    string imagePath = null, fullImagePath = null;
                    if (post.NewPost.Picture != null)
                    {
                        (imagePath, fullImagePath) = await _imagesManager.SaveImage(post.NewPost.Picture);
                    }

                    await _postApiAccess.AddPost(_userId, new PostToAdd
                    {
                        ImageFullPath = fullImagePath,
                        Text = post.NewPost.Text,
                        WhenAdded = DateTime.Now,
                        ImagePath = imagePath
                    });
                }
                return RedirectToAction(nameof(Index), "Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during adding post by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        public async Task<IActionResult> DeletePost(string id)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    await _postApiAccess.DeletePost(_userId, id);
                }
                return RedirectToAction(nameof(Index), "Profile");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during deeting post by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }

        [HttpGet("{userId}/{postId}")]
        public async Task<ActionResult<PostWithCommentToAdd>> ShowComments(string userId, string postId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(postId))
                {
                    return NotFound();
                }
                Post post = await _postApiAccess.GetPost(userId, postId);
                if (post == null)
                {
                    return NotFound();
                }

                ViewData["currentUserProfileId"] = userId.ToString();
                ViewData["loggedUserId"] = _userId.ToString();

                List<BasicUserData> user = new List<BasicUserData>();
                foreach (PostComment userPost in post.PostComments)
                {
                    user.Add(_mapper.Map<BasicUserData>(await _userApiAccess.GetUser(userPost.FromWho.ToString())));
                }
                PostWithCommentToAdd postToReturn = new PostWithCommentToAdd
                {
                    Post = post,
                    Text = string.Empty,
                    Users = user,
                    User = _mapper.Map<BasicUserData>(await _userApiAccess.GetUser(userId))
                };

                return View(postToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading comments on post: {postId} by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpPost("{userId}/{postId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(string userId, string postId, [FromForm] PostWithCommentToAdd postComment)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || postComment == null)
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
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during adding comment on post: {postId} by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpPost("{userId}/{postId}/{commentId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComment(string userId, string postId, string commentId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(commentId))
                {
                    return NotFound();
                }
                await _postApiAccess.DeletePostComment(postId, commentId, userId);
                return RedirectToAction(nameof(ShowComments), new { userId, postId });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during removing comment on post: {postId}, comment: {commentId} by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpGet("EditPost/{userId}/{postId}")]
        public async Task<ActionResult<Post>> EditPost(string userId, string postId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(userId))
                {
                    return NotFound();
                }
                Post post = await _postApiAccess.GetPost(userId, postId);
                if (post == null)
                {
                    return NotFound();
                }
                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during loading post to edit. Post: {postId} by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpPost("EditPost/{userId}/{postId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost([FromBody] Post post)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during edditing post: {post.Id} by user {_userId}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            try
            {
                string mime = image.Substring(image.LastIndexOf('.') + 1);
                return new FileStreamResult(_imagesManager.ImageStream(image), $"image/{mime}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong during streaming image: {image}");
                _logger.LogError($"Exception info: {ex.Message} {ex.Source}");
                return RedirectToAction("Error", "Error");
            }
        }
    }
}