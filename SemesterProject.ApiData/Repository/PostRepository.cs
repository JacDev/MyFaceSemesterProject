using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.AppDbContext;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SemesterProject.ApiData.Repository
{
	public class PostRepository : IPostRepository
	{
		private readonly IApiDbContext _appDbContext;
		private readonly ILogger<PostRepository> _logger;

		public PostRepository(IApiDbContext appDbContext,
			ILogger<PostRepository> logger)
		{
			_appDbContext = appDbContext;
			_logger = logger;
		}
		public async Task AddPostAsync(Post post)
		{
			if (post == null)
			{
				throw new ArgumentNullException(nameof(post));
			}
			if (post.UserId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(post.UserId));
			}
			await _appDbContext.Posts.AddAsync(post);
			await _appDbContext.SaveAsync();
		}
		public async Task DeletePostAsync(Guid postId)
		{
			if (postId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(postId));
			}

			Post postToDelete = _appDbContext.Posts.FirstOrDefault(
				s => s.Id == postId);

			if (postToDelete == null)
			{
				throw new ArgumentNullException(nameof(postToDelete));
			}

			_appDbContext.Posts.Remove(postToDelete);
			await _appDbContext.SaveAsync();
		}
		public List<Post> GetUserPosts(Guid userId)
		{
			_logger.LogInformation("Info z daty");
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}

			List<Post> posts = new List<Post>();

			try
			{
				posts = _appDbContext.Posts.Include(nameof(_appDbContext.PostComments))
					.Include(nameof(_appDbContext.PostLikes))
					.Where(s => s.UserId == userId)
					.ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return posts;
		}
		public Post GetPost(Guid postId)
		{
			if (postId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(postId));
			}

			Post postToReturn = _appDbContext.Posts
				.Include(nameof(_appDbContext.PostComments))
				.Include(nameof(_appDbContext.PostLikes))
				.FirstOrDefault(s => s.Id == postId);

			if (postToReturn == null)
			{
				throw new ArgumentNullException(nameof(postToReturn));
			}
			return postToReturn;
		}
		public async Task UpdatePostAsync(Post post)
		{
			await _appDbContext.SaveAsync();
		}

		public async Task AddLike(PostLike postLike)
		{
			if (postLike == null || postLike.PostId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(postLike));
			}
			_appDbContext.PostLikes.Add(postLike);
			await _appDbContext.SaveAsync();
		}
		public IEnumerable<PostLike> GetLikes(Guid postId)
		{
			if (postId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(postId));
			}

			List<PostLike> likes = _appDbContext.PostLikes
				.Where(p => p.Id == postId)
				.ToList();

			return likes;
		}

		public async Task DeleteLike(string postId, string fromWho)
		{
			if (string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(fromWho))
			{
				throw new ArgumentNullException(nameof(Guid.Empty));
			}

			PostLike like = _appDbContext.PostLikes
				.FirstOrDefault(l => l.PostId.ToString() == postId
				&& l.FromWho.ToString() == fromWho);

			if (like == null)
			{
				throw new ArgumentNullException(nameof(like));
			}
			_appDbContext.PostLikes.Remove(like);
			await _appDbContext.SaveAsync();
		}

		public async Task AddComment(PostComment postComment)
		{
			if (postComment == null || postComment.PostId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(postComment));
			}
			_appDbContext.PostComments.Add(postComment);
			await _appDbContext.SaveAsync();
		}
		public IEnumerable<PostComment> GetComments(Guid postId)
		{
			if (postId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(postId));
			}

			List<PostComment> comments = _appDbContext.PostComments
				.Where(p => p.Id == postId)
				.ToList();

			return comments;
		}

		public async Task DeleteComment(string commentId)
		{
			if (string.IsNullOrWhiteSpace(commentId))
			{
				throw new ArgumentNullException(nameof(Guid.Empty));
			}

			PostComment comment = _appDbContext.PostComments
				.FirstOrDefault(l => l.Id.ToString() == commentId);

			if (comment == null)
			{
				throw new ArgumentNullException(nameof(comment));
			}

			_appDbContext.PostComments.Remove(comment);
			await _appDbContext.SaveAsync();
		}
		public async Task EditComment(PostComment postComment)
		{
			await _appDbContext.SaveAsync();
		}
	}
}