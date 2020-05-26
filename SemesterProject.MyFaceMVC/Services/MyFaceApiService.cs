using System.Net.Http;

namespace SemesterProject.MyFaceMVC.Services
{
	public class MyFaceApiService : IMyFaceApiService
	{
		public HttpClient Client { get; }

		public MyFaceApiService(HttpClient client)
		{
			Client = client;
		}
	}
}