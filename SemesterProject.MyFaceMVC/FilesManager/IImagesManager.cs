using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.FilesManager
{
	public interface IImagesManager
	{
		Task<Tuple<string, string>> SaveImage(IFormFile image);
		FileStream ImageStream(string imageName);
		string ResizeImage(string imageName, int width, int height);
	}
}
