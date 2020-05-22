using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using SemesterProject.MyFaceMVC.Services;
using Microsoft.AspNetCore.Http;
using IdentityModel.Client;
using SemesterProject.MyFaceMVC.Data;
using Microsoft.EntityFrameworkCore;
using SemesterProject.MyFaceMVC.Repository;
using SemesterProject.MyFaceMVC.Hubs;

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
						//options.SignedOutCallbackPath = "/signout-oidc";

						options.GetClaimsFromUserInfoEndpoint = true;
						options.ClaimActions.MapUniqueJsonKey("FirstName", "FirstName");
						options.ClaimActions.MapUniqueJsonKey("LastName", "LastName");

						//options.GetClaimsFromUserInfoEndpoint = true;
						options.Scope.Add("openid");
						options.Scope.Add("profile");
						options.Scope.Add("MyFaceApi");
						options.Scope.Add("userinfo");
					});

			services.AddControllersWithViews();
			services.AddHttpContextAccessor();

			services.AddHttpClient<IMyFaceApiService, MyFaceApiService>(
				async (services, client) =>
				{
					var accessor = services.GetRequiredService<IHttpContextAccessor>();
					var accessToken = await accessor.HttpContext.GetTokenAsync("Cookies", "access_token");
					client.SetBearerToken(accessToken);
					client.BaseAddress = new Uri($"{UrlAddressescs.MyFaceApiUri}");
				});

			services.AddHttpContextAccessor();
			services.AddDbContext<IOnlineUsers, ChatOnlineUsersContext>(
				options => options.UseInMemoryDatabase("OnlineUsers")
				);

			services.AddScoped<IOnlineUsersRepository, OnlineUsersRepository>();
			services.AddSignalR();
		}


		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			app.UseExceptionHandler("/Home/Error");
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
