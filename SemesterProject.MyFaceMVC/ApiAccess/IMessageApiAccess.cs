using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Helpers;
using SemesterProject.ApiData.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ApiAccess
{
	public interface IMessageApiAccess
	{
		Task<HttpResponseMessage> AddMessage(string userId, MessageToAdd messageForAdd);
		Task<IEnumerable<Message>> GetMessages(string userId);
		Task<PagedList<Message>> GetMessagesWith(string userId, string friendId, PaginationParams paginationParams);
	}
}