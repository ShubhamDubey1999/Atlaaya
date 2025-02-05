using System.ComponentModel.DataAnnotations;

namespace Atlaaya.Models
{
	public class ProjectImagesMapping
	{
		[Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string ProjectImagePath { get; set; }
    }
}
