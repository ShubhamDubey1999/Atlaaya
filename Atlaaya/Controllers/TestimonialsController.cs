using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	//[Authorize(Roles = "Customer")]
	public class TestimonialsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AddTestimonial()
		{
			return PartialView();
		}
	}
}
