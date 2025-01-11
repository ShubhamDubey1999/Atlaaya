using Atlaaya.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Atlaaya.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		DataContext _db;
		public HomeController(ILogger<HomeController> logger, DataContext db)
		{
			_logger = logger;
			_db = db;
		}

		public IActionResult Index()
		{
			TestimonialsProject testimonialsProject = new TestimonialsProject();
			testimonialsProject.projects = _db.Projects.ToList();
			testimonialsProject.projects?.ForEach(x =>
			{
				var imagePath = Path.Combine("Projects", x.ProjectImage);
				if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath)))
				{
					x.ProjectImage = "/" + imagePath;
				}
				else
				{
					x.ProjectImage = "https://placehold.co/500x500";
				}
			});
			//testimonialsProject.Testimonials = _db.Testimonials.ToList();
			return View(testimonialsProject);
		}
		public IActionResult About()
		{
			return View();
		}
		public IActionResult Projects()
		{
			return View();
		}
		public IActionResult Careers()
		{
			return View();
		}

		public IActionResult Gallery()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
