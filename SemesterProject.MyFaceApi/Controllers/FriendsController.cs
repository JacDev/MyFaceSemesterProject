﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.ApiData.Repository;

namespace SemesterProject.MyFaceApi.Controllers
{
	[Route("api/users/{userId}/friends")]
	[ApiController]
	public class FriendsController : ControllerBase
	{
		private readonly IRelationRepository _relationRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		public FriendsController(IRelationRepository relationRepository, IMapper mapper, IUserRepository userRepository)
		{
			_relationRepository = relationRepository ??
				throw new ArgumentNullException(nameof(relationRepository));
			_mapper = mapper ??
				throw new ArgumentNullException(nameof(mapper));
			_userRepository = userRepository ??
				throw new ArgumentNullException(nameof(userRepository));
		}
		[HttpOptions]
		public IActionResult GetFriendsOpitons()
		{
			Response.Headers.Add("Allow", "GET,OPTIONS,POST");
			return Ok();
		}

		[HttpGet]
		public ActionResult<List<BasicUserData>> GetFriends(Guid userId)
		{
			if (!_userRepository.CheckIfUserExists(userId))
			{
				return NotFound();
			}

			List<Guid> friendsId = _relationRepository.GetUserRelations(userId)
				.Select(s => (s.FriendId == userId ? s.UserId : s.FriendId))
				.ToList();
			List<User> friends = _userRepository.GetUsers(friendsId);

			return Ok(_mapper.Map<IEnumerable<BasicUserData>>(friends));
		}
		[HttpGet("{friendId}")]
		public ActionResult<bool> CheckIfAreFriends(Guid userId, Guid friendId)
		{
			if (userId == Guid.Empty || friendId == Guid.Empty)
			{
				return NotFound();
			}
			return Ok(_relationRepository.CheckIfFriends(userId, friendId));
		}

		[HttpPost]
		public async Task<IActionResult> AddFriendd(Guid userId, [FromBody] RelationToAdd relation)
		{
			if (userId == Guid.Empty || relation==null)
			{
				return NotFound();
			}

			await _relationRepository.AddRelationAsync(userId, relation.FriendId);
			return NoContent();
		}

		[HttpDelete("{friendId}")]
		public async Task<IActionResult> Delete(Guid userId, string friendId)
		{
			if (userId == Guid.Empty || string.IsNullOrEmpty(friendId))
			{
				return NotFound();
			}

			await _relationRepository.DeleteRelationAsync(userId, Guid.Parse(friendId));
			return NoContent();
		}
	}
}