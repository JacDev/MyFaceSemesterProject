using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SemesterProject.ApiData.Models;
using SemesterProject.MyFaceMVC.Services;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.ViewComponents
{
	[ViewComponent(Name = "NavigationBar")]
	public class NavigationBarViewComponents : ViewComponent
	{
		private readonly IMyFaceApiService _iMyFaceApiService;
		private readonly string _userId;

		public NavigationBarViewComponents(IMyFaceApiService iMyFaceApiService, IHttpContextAccessor httpContextAccessor)
		{
			_iMyFaceApiService = iMyFaceApiService;
			_userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			if (string.IsNullOrWhiteSpace(_userId))
			{
				throw new ArgumentNullException(nameof(_userId));
			}
			UserToReturnWithCounters bacisUserCounters = await _iMyFaceApiService.GetUser(_userId);
			return View(bacisUserCounters);
		}
	}
}