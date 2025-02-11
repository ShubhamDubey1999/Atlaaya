using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlaaya.Controllers
{
	[Authorize]
	public class TestimonialsController : Controller
	{
		private DataContext _db;
		private readonly IWebHostEnvironment _env;

		public TestimonialsController(DataContext db, IWebHostEnvironment env)
		{
			_db = db;
			_env = env;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult AddTestimonial()
		{
			return PartialView();
		}
		[HttpPost]
		public IActionResult AddTestimonial(Testimonial testimonial)
		{
			testimonial.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
			_db.Testimonial.Add(testimonial);
			_db.SaveChanges();
			return Ok();
		}
	}
}
