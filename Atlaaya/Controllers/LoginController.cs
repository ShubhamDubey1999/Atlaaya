using Atlaaya.Data;
using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	public class LoginController : Controller
	{
		private DataContext _db;
		public LoginController(DataContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(User user)
		{
			if (user == null)
			{
				return BadRequest();
			}
			else
			{
				var isUser = _db.User.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
				if (isUser is not null)
				{
					HttpContext.Session.SetInt32("UserId", isUser.Id);
					HttpContext.Session.SetString("Role", isUser.Role);
					return RedirectToAction("Index","Home");
				}
				else
				{
					return BadRequest("Invalid Username or Password");
				}
			}
		}
		[HttpPost]
		public IActionResult Register([FromForm] User user)
		{
			if (user == null)
			{
				return BadRequest("User data is required.");
			}
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors)
												.Select(e => e.ErrorMessage)
												.ToList();
				return BadRequest(new { Errors = errors });
			}

			var existingUser = _db.User.FirstOrDefault(u => u.Email == user.Email);
			if (existingUser != null)
			{
				return BadRequest("Email is already in use.");
			}
			if (!IsPasswordStrong(user.Password))
			{
				return BadRequest("Password does not meet the strength requirements.");
			}
			_db.User.Add(user);
			_db.SaveChanges();

			return Ok("User created successfully.");
		}
		private static bool IsPasswordStrong(string password)
		{
			// A simple example: password must be at least 8 characters, contain a digit, and an uppercase letter
			if (password.Length < 8)
				return false;

			if (!password.Any(char.IsDigit))
				return false;

			if (!password.Any(char.IsUpper))
				return false;

			return true;
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}
}
