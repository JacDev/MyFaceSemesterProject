using AutoMapper;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System.Linq;

namespace SemesterProject.MyFaceApi.Profiles
{
	public class UsersProfiles : Profile
	{
		public UsersProfiles()
		{
			CreateMap<BasicUserData, User>();
			CreateMap<User, UserToReturnWithCounters>()
				.ForMember(
				dest => dest.FriendsCounter,
				opt => opt.MapFrom(src => src.Relations.Count))
				.ForMember(
				dest => dest.PostCounter,
				opt => opt.MapFrom(src => src.Posts.Count))
				.ForMember(
				dest => dest.NewNotificationsCounter,
				opt => opt.MapFrom(src => src.Notifications.Where(s => s.WasSeen == false).Count()));
			CreateMap<User, BasicUserData>();
			CreateMap<BasicUserData, User>();
		}
	}
}
