using Microsoft.AspNetCore.Http.Extensions;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Helpers;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public class MessageApiAccess : IMessageApiAccess
	{
		private readonly IMyFaceApiService _myFaceApiService;

		public MessageApiAccess(IMyFaceApiService myFaceApiService)
		{
			_myFaceApiService = myFaceApiService;
		}
		public async Task<PagedList<Message>> GetMessagesWith(string userId, string friendId, PaginationParams paginationParams)
		{
			var urlParams = new QueryBuilder
				{
					{ nameof(paginationParams.PageNumber), paginationParams.PageNumber.ToString() },
					{ nameof(paginationParams.PageSize), paginationParams.PageSize.ToString() }
				};

			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/messages/{friendId}/{urlParams}");
			return await response.ReadContentAs<PagedList<Message>>();
		}
		public async Task<IEnumerable<Message>> GetMessages(string userId)
		{
			HttpResponseMessage response = await _myFaceApiService.Client.GetFromApiAsync($"api/users/{userId}/messages");
			return await response.ReadContentAs<List<Message>>();
		}
		public async Task<HttpResponseMessage> AddMessage(string userId, MessageToAdd messageToAdd)
		{
			return await _myFaceApiService.Client.PostToApiAsJsonAsync($"api/users/{userId}/messages", messageToAdd);
		}
	}
}
