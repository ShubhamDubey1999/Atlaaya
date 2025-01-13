using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	[Authorize(Roles = "Admin")]
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

			projects.ForEach(x => x.ProjectImage = GetProjectImagePath(x.ProjectImage));

			return View(projects);
		}

		private string GetProjectImagePath(string projectImage)
		{
			if (string.IsNullOrEmpty(projectImage)) return "https://placehold.co/500x500";

			var imagePath = Path.Combine("Projects", projectImage);
			var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

			return System.IO.File.Exists(fullPath) ? "/" + imagePath : "https://placehold.co/500x500";
		}

		[HttpGet]
		public IActionResult Create(int Id)
		{
			var project = Id != 0 ? _db.Projects.FirstOrDefault(p => p.Id == Id) : new Projects();

			if (project is null) return RedirectToAction(nameof(Index));

			project.ProjectImage = GetProjectImagePath(project.ProjectImage);

			return View(project);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Projects projects)
		{
			if (ModelState.IsValid)
			{
				var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "projects");
				if (projects.ProjectImageFile != null && projects.ProjectImageFile.Length > 0)
				{
					Directory.CreateDirectory(uploadDirectory);

					await DeleteOldImageIfExists(uploadDirectory, projects.ProjectImageFile.FileName);

					var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(projects.ProjectImageFile.FileName);
					var filePath = Path.Combine(uploadDirectory, uniqueFileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await projects.ProjectImageFile.CopyToAsync(stream);
					}
					projects.ProjectImage = uniqueFileName;
				}

				if (projects.Id != 0)
				{
					var project = await _db.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id == projects.Id);
					await DeleteOldImageIfExists(uploadDirectory, project.ProjectImage);
					_db.Projects.Update(projects);
				}
				else
				{
					await _db.Projects.AddAsync(projects);
				}

				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(projects);
		}

		private async Task DeleteOldImageIfExists(string uploadDirectory, string currentImage)
		{
			if (!string.IsNullOrEmpty(currentImage))
			{
				var oldImagePath = Path.Combine(uploadDirectory, currentImage);
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}
			}
		}

		public async Task<IActionResult> Delete(int Id)
		{
			var project = _db.Projects.FirstOrDefault(x => x.Id == Id);

			if (project is not null)
			{
				await DeleteOldImageIfExists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "projects"), project.ProjectImage ?? "");
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
