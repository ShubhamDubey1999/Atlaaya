using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	[Authorize(Roles = "Admin")]
	public class TestimonialsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
