using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
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
			try
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

				ResizeImage(fileName, 100, 100);

				return new Tuple<string, string>(fileName, Path.Combine(savePath, fileName));
			}
			catch
			{
				throw;
			}
		}
		public string ResizeImage(string imageName, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			Image imgPhoto = Image.FromFile(Path.Combine(_imagePath, imageName));

			destImage.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.Clamp);
					graphics.DrawImage(imgPhoto, destRect, 0, 0, imgPhoto.Width, imgPhoto.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}
			var newPath = "p" + imageName;
			var savePath = Path.Combine(_imagePath);
			destImage.Save(Path.Combine(savePath, newPath));
			return newPath;
		}
	}
}
