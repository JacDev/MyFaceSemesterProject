using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.AppDbContext;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

namespace SemesterProject.ApiData.Repository
{
	public class PostRepository : IPostRepository
	{
		private readonly IApiDbContext _appDbContext;
		public PostRepository(IApiDbContext appDbContext)
		{
			_appDbContext = appDbContext;
		}
		public async Task AddPostAsync(Post post)
		{
			if (post==null)
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

			var postToDelete = _appDbContext.Posts.FirstOrDefault(
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
			if (userId == Guid.Empty)
			{
				throw new ArgumentNullException(nameof(userId));
			}

			List<Post> posts = new List<Post>();
			try
			{
				posts = _appDbContext.Posts.Include(nameof(_appDbContext.PostComments)).Include(nameof(_appDbContext.PostLikes)).Where(s => s.UserId == userId).ToList();
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
			var postToReturn = _appDbContext.Posts.Include(nameof(_appDbContext.PostComments)).Include(nameof(_appDbContext.PostLikes)).FirstOrDefault(s => s.Id == postId);
			if(postToReturn == null)
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
			if(postLike==null || postLike.PostId == Guid.Empty)
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
			var likes = _appDbContext.PostLikes.Where(p => p.Id == postId).ToList();

			return likes;
		}

		public async Task DeleteLike(string postId, string fromWho)
		{
			if (string.IsNullOrWhiteSpace(postId) || string.IsNullOrWhiteSpace(fromWho))
			{
				throw new ArgumentNullException(nameof(Guid.Empty));
			}

			var like = _appDbContext.PostLikes.FirstOrDefault(l => l.PostId.ToString() == postId && l.FromWho.ToString() == fromWho);
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
			var comments = _appDbContext.PostComments.Where(p => p.Id == postId).ToList();

			return comments;
		}

		public async Task DeleteComment(string commentId)
		{
			if (string.IsNullOrWhiteSpace(commentId))
			{
				throw new ArgumentNullException(nameof(Guid.Empty));
			}

			var comment = _appDbContext.PostComments.FirstOrDefault(l => l.Id.ToString()== commentId);
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
