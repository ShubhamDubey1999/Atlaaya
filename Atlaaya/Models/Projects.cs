using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlaaya.Models
{
	public class Projects
	{
        [Key]
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string? ProjectImage { get; set; }
        public string Value1 { get; set; }
        public string ProjectType { get; set; }
        public string Area { get; set; }
        public string Location { get; set; }
        [NotMapped]
        public IFormFile ProjectImageFile { get; set; }
    }
}
