using AutoMapper;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;

namespace SemesterProject.MyFaceApi.Profiles
{
	public class MessageProfile : Profile
	{
		public MessageProfile()
		{
			CreateMap<MessageToAdd, Message>();
		}
	}
}
