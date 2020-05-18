using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Repository;
using SemesterProject.ApiData.Models;
using Microsoft.AspNetCore.Authorization;

namespace SemesterProject.MyFaceApi.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		public UsersController(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository ??
				throw new ArgumentNullException(nameof(userRepository));
			_mapper = mapper ??
				throw new ArgumentNullException(nameof(mapper));
		}
		[HttpOptions]
		public IActionResult GetUsersOpitons()
		{
			Response.Headers.Add("Allow", "GET,OPTIONS,POST,PATCH");
			return Ok();
		}

		[AllowAnonymous]
		[HttpGet]
		public ActionResult<IQueryable<UserToReturnWithCounters>> GetUsers(string searchName = null)
		{
			List<User> usersFromRepo;
			if (searchName == null)
			{
				usersFromRepo = _userRepository.GetUsers().ToList();
			}
			else
			{
				usersFromRepo = _userRepository.GetUsers(searchName).ToList();
			}
			return Ok(_mapper.Map<IEnumerable<UserToReturnWithCounters>>(usersFromRepo));
		}

		[HttpGet("{userId}", Name = "GetUser")]
		public async Task<ActionResult<UserToReturnWithCounters>> GetUser(Guid userId)
		{
			User userFromRepo = await _userRepository.GetUserAsync(userId);
			if (userFromRepo == null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<UserToReturnWithCounters>(userFromRepo));
		}

		[HttpPost]
		public async Task<ActionResult<UserToReturnWithCounters>> AddUser(BasicUserData user)
		{
			if (user == null)
			{
				return NotFound();
			}
			User userEntity = _mapper.Map<User>(user);
			if (!_userRepository.CheckIfUserExists(user.Id))
			{
				await _userRepository.AddUserAcync(userEntity);
			}

			UserToReturnWithCounters userToReturn = _mapper.Map<UserToReturnWithCounters>(userEntity);

			return CreatedAtRoute("GetUser",
			new { userId = userToReturn.Id },
			userToReturn);
		}

		[HttpPatch("{userId}")]
		public async Task<ActionResult> PartiallyUpdateUser(Guid userId, JsonPatchDocument<BasicUserData> patchDocument)
		{
			User userFromRepo = await _userRepository.GetUserAsync(userId);

			if (userFromRepo == null)
			{
				return NotFound();
			}
			BasicUserData userToPatch = _mapper.Map<BasicUserData>(userFromRepo);

			patchDocument.ApplyTo(userToPatch, ModelState);

			if (!TryValidateModel(userToPatch))
			{
				return ValidationProblem(ModelState);
			}

			_mapper.Map(userToPatch, userFromRepo);

			await _userRepository.UpdateUserAsync(userFromRepo);
			return NoContent();

		}

		[HttpDelete("{userId}")]
		public async Task<ActionResult> DeleteUser(Guid userId)
		{
			User userFromRepo = await _userRepository.GetUserAsync(userId);
			if (userFromRepo == null)
			{
				return NotFound();
			}
			await _userRepository.DeleteUserAsync(userFromRepo);
			return NoContent();
		}
	}
}