using System.ComponentModel.DataAnnotations;

namespace Atlaaya.Models
{
	public class Role
	{
		[Key]
        public int Id { get; set; }
        public string UserRole { get; set; }
    }
}
