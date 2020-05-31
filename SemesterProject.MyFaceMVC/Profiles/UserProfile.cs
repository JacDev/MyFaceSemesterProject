using AutoMapper;
using SemesterProject.ApiData.Models;

namespace SemesterProject.MyFaceMVC.Profiles
{
	public class UserProfile
	{
		public class UsersProfiles : Profile
		{
			public UsersProfiles()
			{
				CreateMap<UserToReturnWithCounters, BasicUserData>();
			}
		}
	}
}
