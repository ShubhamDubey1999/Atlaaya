using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Atlaaya.Models
{
	public class ProjectDocMapping
	{
		[Key]
        public int Id { get; set; }
		public int ProjectId { get; set; }
		public string ProjectDocPath { get; set; }
        [NotMapped]
        public string ProjectDocName { get; set; }
    }
}
