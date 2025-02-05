using Atlaaya.HelperMethods;
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
			projects.ForEach(x => x.ProjectImage = ImageHelper.GetProjectImagePath(x.ProjectImage));
			return View(projects);
		}

		[HttpGet]
		public IActionResult Create(int Id)
		{
			var project = Id != 0 ? _db.Projects.FirstOrDefault(p => p.Id == Id) : new Projects();
			if (project is null)
			{
				return RedirectToAction(nameof(Index));
			}
			project.ProjectImage = ImageHelper.GetProjectImagePath(project.ProjectImage);
			project.ProjectDocList = [.. _db.ProjectDocMapping.Where(x => x.ProjectId == Id)];
			project.ProjectImagesList = [.. _db.ProjectImagesMapping.Where(x => x.ProjectId == Id)];
			return View(project);
		}

		public IActionResult ProjectImages(int ProjectId)
		{
			var projectImagesObj = new List<ProjectImagesMapping>();
			var projectImages = _db.ProjectImagesMapping.Where(x => x.ProjectId == ProjectId).ToList();
			if (projectImages is null)
			{
				return RedirectToAction(nameof(Index));
			}
			projectImagesObj = (from s in projectImages
								select new ProjectImagesMapping
								{
									ProjectImagePath = ImageHelper.GetProjectImagePath(s.ProjectImagePath)
								}).ToList();
			ViewBag.ProjectName = _db.Projects.FirstOrDefault(x => x.Id == ProjectId)?.ProjectName;
			return View(projectImagesObj);
		}

		public IActionResult ProjectDocuments(int ProjectId)
		{
			var ProjectDocObj = new List<ProjectDocMapping>();
			var projectDocList = _db.ProjectDocMapping.Where(x => x.ProjectId == ProjectId).ToList();
			if (projectDocList is null)
			{
				return RedirectToAction(nameof(Index));
			}
			ProjectDocObj = (from s in projectDocList
							 select new ProjectDocMapping
							 {
								 ProjectDocName = s.ProjectDocPath.Split('\\').LastOrDefault(),
							 }).ToList();

			ViewBag.ProjectName = _db.Projects.FirstOrDefault(x => x.Id == ProjectId)?.ProjectName;
			return View(ProjectDocObj);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Projects projects)
		{
			if (ModelState.IsValid)
			{
				var isUpdate = projects.Id != 0;
				var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Projects");

				if (!Directory.Exists(uploadDirectory))
				{
					Directory.CreateDirectory(uploadDirectory);
				}

				var subProjectDirectory = Path.Combine(uploadDirectory, projects.ProjectName);

				if (isUpdate && Directory.Exists(subProjectDirectory))
				{
					Directory.Delete(subProjectDirectory, true);
				}

				if (!Directory.Exists(subProjectDirectory))
				{
					Directory.CreateDirectory(subProjectDirectory);
				}

				var subProjectThumbnailDirectory = Path.Combine(subProjectDirectory, "thumbnail");
				var subProjectImagesDirectory = Path.Combine(subProjectDirectory, "images");
				var subProjectDocsDirectory = Path.Combine(subProjectDirectory, "docs");

				Directory.CreateDirectory(subProjectThumbnailDirectory);
				Directory.CreateDirectory(subProjectImagesDirectory);
				Directory.CreateDirectory(subProjectDocsDirectory);

				if (projects.ProjectImageFile != null)
				{
					var thumbnailFilePath = Path.Combine(subProjectThumbnailDirectory, projects.ProjectImageFile.FileName);
					using (var stream = new FileStream(thumbnailFilePath, FileMode.Create))
					{
						await projects.ProjectImageFile.CopyToAsync(stream);
					}
					projects.ProjectImage = Path.Combine("Projects", projects.ProjectName, "thumbnail", projects.ProjectImageFile.FileName);
				}

				if (projects.ProjectImagesFiles != null && projects.ProjectImagesFiles.Any())
				{
					foreach (var file in projects.ProjectImagesFiles)
					{
						var imageFilePath = Path.Combine(subProjectImagesDirectory, file.FileName);
						using (var stream = new FileStream(imageFilePath, FileMode.Create))
						{
							await file.CopyToAsync(stream);
						}
					}
				}

				if (projects.ProjectDocFiles != null && projects.ProjectDocFiles.Any())
				{
					foreach (var file in projects.ProjectDocFiles)
					{
						var docFilePath = Path.Combine(subProjectDocsDirectory, file.FileName);
						using (var stream = new FileStream(docFilePath, FileMode.Create))
						{
							await file.CopyToAsync(stream);
						}
					}
				}

				if (!isUpdate)
				{
					_db.Projects.Add(projects);
					await _db.SaveChangesAsync();
				}
				else
				{
					_db.Projects.Update(projects);
					await _db.SaveChangesAsync();
				}

				string dbFilePath = Path.Combine("Projects", projects.ProjectName);

				if (projects.ProjectImagesFiles != null && projects.ProjectImagesFiles.Any())
				{
					foreach (var file in projects.ProjectImagesFiles)
					{
						var imagePath = Path.Combine(dbFilePath, "images", file.FileName);
						var imageMapping = new ProjectImagesMapping
						{
							ProjectId = projects.Id,
							ProjectImagePath = imagePath
						};
						if (isUpdate)
						{
							_db.ProjectImagesMapping.Update(imageMapping);
						}
						else
						{
							_db.ProjectImagesMapping.Add(imageMapping);
						}
					}
				}

				if (projects.ProjectDocFiles != null && projects.ProjectDocFiles.Any())
				{
					foreach (var file in projects.ProjectDocFiles)
					{
						var docPath = Path.Combine(dbFilePath, "docs", file.FileName);
						var docMapping = new ProjectDocMapping
						{
							ProjectId = projects.Id,
							ProjectDocPath = docPath
						};
						if (isUpdate)
						{
							_db.ProjectDocMapping.Update(docMapping);
						}
						else
						{
							_db.ProjectDocMapping.Add(docMapping);
						}
					}
				}

				await _db.SaveChangesAsync();

				return View(projects);
			}

			return View(projects);
		}


		public async Task<IActionResult> Delete(int Id)
		{
			var project = _db.Projects.FirstOrDefault(x => x.Id == Id);

			if (project is not null)
			{
				await ImageHelper.DeleteOldImageIfExists(project.ProjectImage ?? "");
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
