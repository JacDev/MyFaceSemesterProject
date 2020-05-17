using SemesterProject.ApiData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.ApiData.Repository
{
	public interface IPostRepository
	{
		List<Post> GetUserPosts(Guid userId);
		Task AddPostAsync(Post post);
		Task DeletePostAsync(Guid postId);
		Post GetPost(Guid postId);
		Task UpdatePostAsync(Post post);

		Task AddLike(PostLike postLike);
		IEnumerable<PostLike> GetLikes(Guid postId);
		Task DeleteLike(string postId, string fromWho);

		Task AddComment(PostComment postComment);
		IEnumerable<PostComment> GetComments(Guid postId);
		Task DeleteComment(string commentId);
		Task EditComment(PostComment postComment);
	}
}
