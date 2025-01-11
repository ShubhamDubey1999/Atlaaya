using System.ComponentModel.DataAnnotations;

namespace Atlaaya.Models
{
    public class Enquire
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Location { get; set; }
        [Required]
        public string Budget { get; set; }
        public string? Message { get; set; }
    }
}
