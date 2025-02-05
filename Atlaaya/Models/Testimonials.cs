using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlaaya.Models
{
	public class Testimonial
	{
		[Key]
		public int Id { get; set; }
		public string UserName { get; set; }
		public string InteriorName { get; set; }
		public string InteriorType { get; set; }
		public string InteriorReview { get; set; }
		public bool IsActive { get; set; }
		public int UserId { get; set; }
		public string Email { get; set; }
		public string InteriorImage { get; set; }

		[NotMapped]
		public IFormFile InteriorImageFile { get; set; }
	}
}