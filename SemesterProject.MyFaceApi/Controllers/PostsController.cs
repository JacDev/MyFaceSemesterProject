using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.ApiData.Repository;

namespace SemesterProject.MyFaceApi.Controllers
{
    [Route("api/users/{userId}/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<PostsController> _logger;
        private readonly IMapper _mapper;
        public PostsController(IPostRepository postRepository,
            IMapper mapper, 
            IUserRepository userRepository,
            ILogger<PostsController> logger)
        {
            _postRepository = postRepository ??
                throw new ArgumentNullException(nameof(postRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ??
                throw new ArgumentNullException(nameof(userRepository));
            _logger = logger;
        }
        [HttpOptions]
        public IActionResult GetPostsOpitons()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetPosts(Guid userId)
        {
            var userID = User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
            _logger.LogInformation("{UserId} in api", userID);
            IEnumerable<Post> userPosts = _postRepository.GetUserPosts(userId);
            return Ok(userPosts);
        }

        [HttpGet("{postId}", Name = "GetPost")]
        public ActionResult<Post> GetPost(Guid postId)
        {
            Post postToReturn = _postRepository.GetPost(postId);
            if (postToReturn == null)
            {
                return NotFound();
            }
            return Ok(postToReturn);
        }

        [HttpPost]
        public async Task<ActionResult> AddPost(Guid userId, PostToAdd post)
        {
            if (userId == Guid.Empty || post == null)
            {
                return NotFound();
            }
            Post postEntity = _mapper.Map<Post>(post);
            postEntity.UserId = userId;
            await _postRepository.AddPostAsync(postEntity);
            return CreatedAtRoute("GetPost",
                new { userId, postId = postEntity.Id },
                postEntity);
        }

        [HttpPatch("{postId}")]
        public async Task<ActionResult> PartiallyUpdatePost(string postId, PostToUpdate posToToUpdate)
        {
            if (postId == null)
            {
                return NotFound();
            }
            Post postFromRepo = _postRepository.GetPost(Guid.Parse(postId));

            if (postFromRepo == null)
            {
                return NotFound();
            }
            postFromRepo.Text = posToToUpdate.Text;

            await _postRepository.UpdatePostAsync(postFromRepo);
            return NoContent();

        }

        [HttpDelete("{postId}")]
        public async Task<ActionResult> DeletePost(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                return NotFound();
            }

            await _postRepository.DeletePostAsync(postId);
            return NoContent();
        }
        [HttpGet("{postId}/likes")]
        public ActionResult GetLikes(Guid postId)
        {
            var likes = _postRepository.GetLikes(postId);
            if (likes == null)
            {
                return NotFound();
            }
            return Ok(likes);
        }

        [HttpPost("{postId}/likes")]
        public async Task<ActionResult> AddLike(Guid postId, [FromBody] PostLike postLike)
        {
            if (postId == Guid.Empty || postLike==null)
            {
                return NotFound();
            }
            await _postRepository.AddLike(postLike);
            return Ok();
        }
        [HttpDelete("{postId}/likes/{fromWho}")]
        public async Task<ActionResult> DeleteLike(string postId, string fromWho)
        {
            if (string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(fromWho))
            {
                return NotFound();
            }
            await _postRepository.DeleteLike(postId, fromWho);
            return Ok();
        }

        [HttpGet("{postId}/comments")]
        public ActionResult GetComments(Guid postId)
        {
            var comment = _postRepository.GetComments(postId);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost("{postId}/comments")]
        public async Task<ActionResult> AddComment(Guid postId, [FromBody] PostComment postComment)
        {
            if (postId == Guid.Empty || postComment == null)
            {
                return NotFound();
            }
            await _postRepository.AddComment(postComment);
            return Ok();
        }
        [HttpDelete("{postId}/comments/{commentId}")]
        public async Task<ActionResult> DeleteComment(string commentId)
        {
            if (string.IsNullOrWhiteSpace(commentId))
            {
                return NotFound();
            }
            await _postRepository.DeleteComment(commentId);
            return Ok();
        }
    }
}