using Atlaaya.HelperMethods;
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
				var imagePath = Path.Combine(x.ProjectImage);
				if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath)))
				{
					x.ProjectImage = ImageHelper.GetProjectImagePath(imagePath);
				}
				else
				{
					x.ProjectImage = "https://placehold.co/500x500";
				}
			});
			return View(testimonialsProject);
		}
		public IActionResult About()
		{
			AboutDTO testimonialDTO = new()
			{
				Teams = [.. _db.Team]
			};
			testimonialDTO?.Teams?.ForEach(x => { x.Image = ImageHelper.GetProjectImagePath(x.Image); });
			return View(testimonialDTO);
		}
		public IActionResult Projects()
		{
			TestimonialsProject testimonialsProject = new TestimonialsProject();
			testimonialsProject.projects = _db.Projects.ToList();
			testimonialsProject.projects?.ForEach(x =>
			{
				var imagePath = Path.Combine(x.ProjectImage);
				if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath)))
				{
					x.ProjectImage = ImageHelper.GetProjectImagePath(imagePath);
				}
				else
				{
					x.ProjectImage = "https://placehold.co/500x500";
				}
			});
			return View(testimonialsProject);
		}
		[HttpGet]
		public IActionResult ProjectDetail(int ProjectId)
		{
			var project = _db.Projects.FirstOrDefault(x => x.Id == ProjectId);
			if (project is null)
			{
				return RedirectToAction(nameof(Index));
			}
			var projectImages = _db.ProjectImagesMapping.Where(x => x.ProjectId == ProjectId).ToList();
			project.ProjectImagesList = (from s in projectImages
										 select new ProjectImagesMapping
										 {
											 ProjectImagePath = ImageHelper.GetProjectImagePath(s.ProjectImagePath)
										 }).ToList();
			return View(project);
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
