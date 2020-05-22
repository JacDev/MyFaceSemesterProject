using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SemesterProject.IdentityServer.Data;
using SemesterProject.IdentityServer.Entities;
using SemesterProject.IdentityServer.Services;

namespace SemesterProject.IdentityServer
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
			services.AddRazorPages();
			services.AddControllersWithViews();

			var connectionString = Configuration.GetConnectionString("DefaultConnection");

			//identity database
			services.AddDbContext<AppDbContext>(config =>
			{
				config.UseSqlServer(connectionString);
			}
			);

			services.AddIdentity<AppUser, IdentityRole<Guid>>(config =>
			{
				config.Password.RequiredLength = 1;
				config.Password.RequireDigit = false;
				config.Password.RequireNonAlphanumeric = false;
				config.Password.RequireUppercase = false;
				config.Password.RequiredUniqueChars = 0;
				config.Password.RequireLowercase = false;
				config.SignIn.RequireConfirmedEmail = true;

			})
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(config =>
			{
				config.Cookie.Name = "IdentityServer.Cookie";
				config.LoginPath = "/Auth/Login";
				config.LogoutPath = "/Auth/Logout";
				
			});

			//configuration database
			var assembly = typeof(Startup).Assembly.GetName().Name;

			services.AddIdentityServer()
				.AddAspNetIdentity<AppUser>()
				.AddConfigurationStore(options =>
				{
					options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
						  sql => sql.MigrationsAssembly(assembly));
				})
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
						  sql => sql.MigrationsAssembly(assembly));
				})
				.AddDeveloperSigningCredential(); //add rsa key
			services.AddTransient<IEmailSender, EmailSender>();

		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseIdentityServer();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
