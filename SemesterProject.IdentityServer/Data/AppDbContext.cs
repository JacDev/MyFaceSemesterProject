using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SemesterProject.IdentityServer.Entities;
using System;

namespace SemesterProject.IdentityServer.Data
{
	public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
	}
}
