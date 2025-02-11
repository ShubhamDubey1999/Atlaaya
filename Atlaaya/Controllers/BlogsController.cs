using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	public class BlogsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		[Authorize(Roles = "Admin")]
		public IActionResult CreateBlog()
		{
			return View();
		}
	}
}
