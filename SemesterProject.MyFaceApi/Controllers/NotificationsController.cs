using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using SemesterProject.ApiData.Repository;

namespace SemesterProject.MyFaceApi.Controllers
{
	[Route("api/users/{userId}/notifications")]
	[ApiController]
	public class NotificationsController : ControllerBase
	{
		private readonly INotificationRepository _notificationRepository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		public NotificationsController(INotificationRepository notificationRepository, IMapper mapper, IUserRepository userRepository)
		{
			_notificationRepository = notificationRepository ??
				throw new ArgumentNullException(nameof(notificationRepository));
			_mapper = mapper ??
				throw new ArgumentNullException(nameof(mapper));
			_userRepository = userRepository ??
				throw new ArgumentNullException(nameof(userRepository));
		}
		public IActionResult GetNotificationsOpitons()
		{
			Response.Headers.Add("Allow", "GET,OPTIONS,POST");
			return Ok();
		}
		[HttpGet("{eventId}/{friendId}")]
		public ActionResult<Notification> GetPost(Guid userId, Guid eventId, Guid friendId)
		{
			if (userId == Guid.Empty || eventId == Guid.Empty || friendId == Guid.Empty)
			{
				return NotFound();
			}
			Notification notificationToReturn = _notificationRepository.GetNotification(userId, friendId, eventId);
			if (notificationToReturn == null)
			{
				return NotFound();
			}
			return Ok(notificationToReturn);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<NotificationWithBasicFromWhoData>>> GetNotificationsWithBasicFromWhoData(Guid userId)
		{
			if (userId == Guid.Empty || !_userRepository.CheckIfUserExists(userId))
			{
				return NotFound();
			}
			IEnumerable<Notification> userNotifications = _notificationRepository.GetUserNotifications(userId);

			List<NotificationWithBasicFromWhoData> notificationsToReturn = new List<NotificationWithBasicFromWhoData>();

			foreach (var notification in userNotifications)
			{
				User user = await _userRepository.GetUserAsync(notification.FromWho);
				var userToReturn = _mapper.Map<BasicUserData>(user);
				notificationsToReturn.Add(new NotificationWithBasicFromWhoData
				{
					Notification = notification,
					User = userToReturn
				});
			};

			return Ok(notificationsToReturn);
		}
		[HttpPost]
		public async Task<ActionResult> AddMNotification([FromBody] Notification notification)
		{
			if (notification == null || !_userRepository.CheckIfUserExists(notification.UserId))
			{
				return NotFound();
			}
	 
			await _notificationRepository.AddNotificationAsync(notification);
			return NoContent();
		}
		[HttpPatch("{notificationId}")]
		public async Task<IActionResult> MarNotificationAsSeen(Guid notificationId, Notification notification)
		{
			if(notificationId == Guid.Empty)
			{
				return NotFound();
			}
			await _notificationRepository.MarkNotificationAsSeen(notificationId);
			return Ok();
		}
		[HttpDelete("{notificationId}")]
		public async Task<ActionResult> DeleteNotification(Guid notificationId)
		{
			if (notificationId == Guid.Empty)
			{
				return NotFound();
			}

			await _notificationRepository.DeleteNotificationAsync(notificationId);
			return NoContent();
		}
	}
}