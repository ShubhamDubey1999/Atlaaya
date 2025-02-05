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
        public string Description { get; set; }
        [NotMapped]
        public IFormFile? ProjectImageFile { get; set; }
		[NotMapped]
		public IEnumerable<IFormFile>? ProjectImagesFiles { get; set; }
		[NotMapped]
		public IEnumerable<IFormFile>? ProjectDocFiles { get; set; }
        [NotMapped]
        public List<ProjectImagesMapping>? ProjectImagesList { get; set; }
        [NotMapped]
        public List<ProjectDocMapping>? ProjectDocList { get; set; }
    }
}
