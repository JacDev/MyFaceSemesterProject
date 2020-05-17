using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;

namespace SemesterProject.MyFaceApi.Profiles
{
	public class FriendsProfile : Profile
	{
		public FriendsProfile()
		{
			CreateMap<User, UserToReturnAsFriend>();
			CreateMap<User, UserToReturn>();
		}
	}
}
