using System.ComponentModel.DataAnnotations;

namespace Atlaaya.Models
{
	public class Career
	{
		[Key]
        public int Id { get; set; }
        public string JobPosition { get; set; }
        public string JobType { get; set; }
        public string JobLocation { get; set; }
        public string JobDescription { get; set; }
        public string Responsibilties { get; set; }
        public string Skills { get; set; }
        public IFormFile CareerImageFile { get; set; }
        public string CareerImage { get; set; }
    }
}
