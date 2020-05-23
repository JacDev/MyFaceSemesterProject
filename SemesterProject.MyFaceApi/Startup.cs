using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using SemesterProject.ApiData.AppDbContext;
using SemesterProject.ApiData.Repository;


namespace SemesterProject.MyFaceApi
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
		
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.Authority = UrlAddressescs.IdentityServerUri;
				options.Audience = "MyFaceApi";
			});

			services.AddControllers(options =>
			{
				options.Filters.Add(new AuthorizeFilter());
				options.ReturnHttpNotAcceptable = true;
			})
				.AddNewtonsoftJson(setupAction =>
				{
					setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});


			services.AddDbContext<IApiDbContext, ApiDbContext>(
				options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IRelationRepository, RelationRepository>();
			services.AddScoped<IPostRepository, PostRepository>();
			services.AddScoped<IMessageRepository, MessageRepository>();
			services.AddScoped<INotificationRepository, NotificationRepository>();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}
		
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
