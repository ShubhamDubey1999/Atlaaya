using Atlaaya.HelperMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	[Authorize(Roles = "Admin")]
	public class TeamController : Controller
	{
		private readonly ILogger<TeamController> _logger;
		DataContext _db;
		private readonly IWebHostEnvironment _env;
		public TeamController(ILogger<TeamController> logger, DataContext db, IWebHostEnvironment env)
		{
			_logger = logger;
			_db = db;
			_env = env;
		}
		public async Task<IActionResult> Index()
		{
			var team = await _db.Team.ToListAsync();
			if (team?.Count > 0)
			{
				team.ForEach(x => x.Image = ImageHelper.GetProjectImagePath(x.Image));
			}
			return View(team);
		}
		[HttpGet]
		public async Task<IActionResult> AddTeamMember(int Id)
		{
			Team team = new();
			if (Id != 0)
			{
				team = await _db.Team.FirstOrDefaultAsync(x => x.Id == Id);
				if (team is not null)
				{
					team.Image = ImageHelper.GetProjectImagePath(team.Image);
				}
			}
			return Ok(team);
		}
		[HttpPost]
		public async Task<IActionResult> AddTeamMember(Team team)
		{
			try
			{
				if (team.TeamMemberImage != null && team.TeamMemberImage.Length > 0)
				{
					var TeamDirectory = Path.Combine(_env.WebRootPath, "Team");
					if (!Directory.Exists(TeamDirectory))
					{
						Directory.CreateDirectory(TeamDirectory);
					}
					// Save the image to the server
					var fileName = Path.GetFileName(team.TeamMemberImage.FileName);
					var filePath = Path.Combine(_env.WebRootPath, "Team", fileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await team.TeamMemberImage.CopyToAsync(stream);
					}
					team.Image = Path.Combine("Team", fileName);
				}
				if (team.Id != 0)
				{
					_db.Team.Update(team);
				}
				else
				{
					_db.Team.Add(team);
				}
				await _db.SaveChangesAsync();
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
		public async Task<IActionResult> DeleteTeamMember(int Id)
		{
			var team = await _db.Team.Where(x => x.Id == Id).FirstOrDefaultAsync();
			if (team != null)
			{
				await ImageHelper.DeleteOldImageIfExists(team.Image ?? "");
				_db.Team.Remove(team);
				await _db.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
