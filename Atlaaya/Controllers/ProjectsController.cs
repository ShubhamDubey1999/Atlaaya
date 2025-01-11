using Atlaaya.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Atlaaya.Controllers
{
	public class ProjectsController : Controller
	{
		private DataContext _db;
		private readonly IWebHostEnvironment _env;
		public ProjectsController(DataContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;
		}
		public async Task<IActionResult> Index()
		{
			var projects = await _db.Projects.ToListAsync();
			projects.ForEach(x =>
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
			return View(projects);
		}


		[HttpGet]
		public IActionResult Create(int Id)
		{
			Projects project = new Projects();
			if (Id != 0)
			{
				project = _db.Projects.FirstOrDefault(p => p.Id == Id);
				if (project is null)
				{
					return RedirectToAction(nameof(Index));
				}
				project.ProjectImage = string.IsNullOrEmpty(project.ProjectImage) ? "https://placehold.co/500x500" : "/" + Path.Combine("Projects", project.ProjectImage);
			}
			else
			{
				project.ProjectImage = "https://placehold.co/500x500";
			}
			return View(project);
		}
		[HttpPost]
		public async Task<IActionResult> Create(Projects projects)
		{
			if (ModelState.IsValid)
			{
				if (projects.ProjectImageFile != null && projects.ProjectImageFile.Length > 0)
				{
					var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "projects");
					if (!Directory.Exists(uploadDirectory))
					{
						Directory.CreateDirectory(uploadDirectory);
					}
					var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(projects.ProjectImageFile.FileName);
					//var filePath = Path.Combine(uploadDirectory, uniqueFileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "projects", projects.ProjectImageFile.FileName);
					if (System.IO.File.Exists(filePath))
					{
						System.IO.File.Delete(filePath);
					}
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await projects.ProjectImageFile.CopyToAsync(stream);
					}
					projects.ProjectImage = projects.ProjectImageFile.FileName;
				}
				_db.Projects.Add(projects);
				_db.SaveChanges();
			}
			return RedirectToAction(nameof(Index));

		}
		public IActionResult Delete(int Id)
		{
			var project = _db.Projects.FirstOrDefault(x => x.Id == Id);
			if (project is not null)
			{
				_db.Projects.Remove(project);
				_db.SaveChanges();
			}
			return RedirectToAction(nameof(Index));
		}
		public IActionResult Enquiries()
		{
			var enquiries = _db.Enquire.ToList();
			return View(enquiries);
		}
	}
}
