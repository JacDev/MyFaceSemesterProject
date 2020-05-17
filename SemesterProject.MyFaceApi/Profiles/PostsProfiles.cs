using AutoMapper;
using SemesterProject.ApiData.Entities;
using SemesterProject.ApiData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceApi.Profiles
{
	public class PostsProfiles : Profile
	{
		public PostsProfiles()
		{
			CreateMap<PostToAdd, Post>();
			CreateMap<PostToUpdate, Post>();
			CreateMap<Post, PostToUpdate>();
		}
	}
}
