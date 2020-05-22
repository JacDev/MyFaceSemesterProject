//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using SemesterProject.MyFaceApi;
//using Serilog;
//using Serilog.Formatting.Json;

//namespace SemesterProject.MyFaceApi
//{
//	public class Program
//	{
//		public static IConfiguration Configurationn { get; } = new ConfigurationBuilder()
//	.SetBasePath(Directory.GetCurrentDirectory())
//	.AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
//	.AddEnvironmentVariables()
//	.Build();
//		public static void Main(string[] args)
//		{
//			Log.Logger = new LoggerConfiguration()
//				.ReadFrom.Configuration(Configurationn)
//				.WriteTo.File(new JsonFormatter(), @"c:\temp\logs\my-face.json", shared: true)
//				.CreateLogger();

//			try
//			{
//				Log.Information("Starting web host");
//				CreateHostBuilder(args).Build().Run();
//			}
//			catch (Exception ex)
//			{
//				Log.Fatal(ex, "Host terminated unexpectedly");
//			}
//			finally
//			{
//				Log.CloseAndFlush();
//			}
//		}

//		public static IHostBuilder CreateHostBuilder(string[] args) =>
//			Host.CreateDefaultBuilder(args)
//				.ConfigureWebHostDefaults(webBuilder =>
//				{
//					webBuilder.UseStartup<Startup>();
//				});
//	}
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SemesterProject.MyFaceApi;

namespace SemesterProject.IdentityServer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHostBuilder(args).Build();


			host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
