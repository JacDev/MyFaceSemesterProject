using System.Net.Http;

namespace SemesterProject.MyFaceMVC.Services
{
	public interface IMyFaceApiService
	{
		HttpClient Client { get; }
	}
}