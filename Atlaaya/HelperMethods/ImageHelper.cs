namespace Atlaaya.HelperMethods
{
	public static class ImageHelper
	{
		public static string GetProjectImagePath(string projectImage)
		{
			if (string.IsNullOrEmpty(projectImage)) return "https://placehold.co/500x500";

			var imagePath = Path.Combine(projectImage);
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

			return System.IO.File.Exists(fullPath) ? "/" + imagePath : "https://placehold.co/500x500";
		}
		public static async Task DeleteOldImageIfExists(string currentImage)
		{
			if (!string.IsNullOrEmpty(currentImage))
			{
				var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", currentImage);
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}
			}
		}
	}
}
