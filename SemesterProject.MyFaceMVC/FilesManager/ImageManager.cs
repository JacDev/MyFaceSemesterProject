using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.FilesManager
{
	public class ImageManager : IImagesManager
	{
		private readonly string _imagePath;
		public ImageManager(IConfiguration config)
		{
			_imagePath = config["Path:Images"];

		}

		public FileStream ImageStream(string imageName)
		{
			return new FileStream(Path.Combine(_imagePath, imageName), FileMode.Open, FileAccess.Read);
		}

		public async Task<Tuple<string, string>> SaveImage(IFormFile image)
		{
			var savePath = Path.Combine(_imagePath);
			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}
			var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
			var fileName = $"img_{DateTime.Now:dd-MM-yyy-HH-mm-ss}{mime}";

			var fileStream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create);

			await image.CopyToAsync(fileStream);
			fileStream.Close();

			return new Tuple<string, string>(fileName, Path.Combine(savePath, fileName));
		}
	}
}
