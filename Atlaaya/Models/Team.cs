using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlaaya.Models
{
	public class Team
	{
		[Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile TeamMemberImage { get; set; }
    }
}
