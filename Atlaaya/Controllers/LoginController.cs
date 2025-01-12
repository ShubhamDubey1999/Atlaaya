using Atlaaya.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
			return RedirectToAction("Index", "Home");
		}
		[HttpPost]
		public async Task<IActionResult> Login(User user)
		{
			if (user == null)
			{
				return BadRequest("Invalid Username or Password");
			}
			else
			{
				var isUser = _db.User.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
				if (isUser is not null)
				{
					HttpContext.Session.SetInt32("UserId", isUser.Id);
					HttpContext.Session.SetString("Role", isUser.Role);
					var claims = new[]
					{
						new Claim(ClaimTypes.Name, isUser.Email),
						new Claim(ClaimTypes.Role, isUser.Role)
					};

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var authenticationProperties = new AuthenticationProperties
					{
						IsPersistent = true // Persist the authentication cookie
					};

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity), authenticationProperties);
					return RedirectToAction("Index", "Home");
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
		[HttpPost]
		public IActionResult Enquire(Enquire enquire)
		{
			if (ModelState.IsValid)
			{
				_db.Enquire.Add(enquire);
				_db.SaveChanges();
				return Ok(true);
			}
			else
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors)
									   .Select(e => e.ErrorMessage)
									   .ToList();
				return BadRequest(errors);
			}
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Home");
		}
	}
}
