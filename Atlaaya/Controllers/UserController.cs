using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	public class UserController : Controller
	{
		private readonly DataContext _db;
		public UserController(DataContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			var users = _db.User.ToList();
			return View(users);
		}
		public async Task<IActionResult> Create(int Id)
		{
			var user = await _db.User.FirstOrDefaultAsync(x => x.Id == Id);
			ViewBag.UserRole = GetUserRoles();
			return PartialView(user);
		}
		private static List<Role> GetUserRoles()
		{
			var roles = new List<Role>() {
				new() {Id = 0,UserRole = "--select--"},
				new() {Id = 1,UserRole = "Admin"},
				new() {Id = 2,UserRole = "Customer"}
			};
			return roles;
		}
		[HttpPost]
		public IActionResult Create(User user)
		{
			return View(user);
		}
		public IActionResult Delete()
		{
			return RedirectToAction("Index");
		}
	}
}
