using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using SemesterProject.MyFaceMVC.Services;
using Microsoft.AspNetCore.Http;
using IdentityModel.Client;
using SemesterProject.MyFaceMVC.Data;
using Microsoft.EntityFrameworkCore;
using SemesterProject.MyFaceMVC.Repository;
using SemesterProject.MyFaceMVC.Hubs;
using SemesterProject.MyFaceMVC.FilesManager;
using SemesterProject.MyFaceMVC.ApiAccess;
using Microsoft.AspNetCore.Mvc.Authorization;
using AutoMapper;

namespace SemesterProject.MyFaceMVC
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		public IConfiguration Configuration { get; }
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultScheme = "Cookies";
				options.DefaultChallengeScheme = "oidc";
			})
				.AddCookie("Cookies")
				.AddOpenIdConnect("oidc", options =>
					{
						options.Authority = UrlAddressescs.IdentityServerUri;

						options.ClientId = "MyFaceClient";
						options.ClientSecret = "MyFaceClientSecret";
						options.ResponseType = "code";

						options.SaveTokens = true;
						options.CallbackPath = "/signin-oidc";
						
						options.GetClaimsFromUserInfoEndpoint = true;
						options.ClaimActions.MapUniqueJsonKey("FirstName", "FirstName");
						options.ClaimActions.MapUniqueJsonKey("LastName", "LastName");

						options.Scope.Add("openid");
						options.Scope.Add("profile");
						options.Scope.Add("MyFaceApi");
						options.Scope.Add("userinfo");
					});

			services.AddControllersWithViews(options=> options.Filters.Add(new AuthorizeFilter()));
			services.AddHttpContextAccessor();

			services.AddHttpClient<IMyFaceApiService, MyFaceApiService>(
				async (services, client) =>
				{
					var accessor = services.GetRequiredService<IHttpContextAccessor>();
					var accessToken = await accessor.HttpContext.GetTokenAsync("Cookies", "access_token");
					client.SetBearerToken(accessToken);
					client.BaseAddress = new Uri($"{UrlAddressescs.MyFaceApiUri}");
				});

			services.AddScoped<IUserApiAccess, UserApiAccess>();
			services.AddScoped<IFriendApiAccess, FriendApiAccess>();
			services.AddScoped<IMessageApiAccess, MessageApiAccess>();
			services.AddScoped<IPostApiAccess, PostApiAccess>();
			services.AddScoped<INotificationApiAccess, NotificationApiAccess>();

			services.AddHttpContextAccessor();
			services.AddDbContext<IOnlineUsers, ChatOnlineUsersContext>(
				options => options.UseInMemoryDatabase("OnlineUsers")
				);

			services.AddScoped<IOnlineUsersRepository, OnlineUsersRepository>();

			services.AddSignalR();
			services.AddScoped<IImagesManager, ImageManager>();

			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseExceptionHandler("/Error/Error");
			app.UseHsts();

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Profile}/{action=Index}/{id?}");
				endpoints.MapHub<NotificationHub>("/notificationHub");
			});
		}
	}
}
