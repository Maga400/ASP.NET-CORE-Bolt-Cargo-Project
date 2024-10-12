using System.ComponentModel.DataAnnotations;

namespace BoltCargo.WebUI.Dtos
{
    public class LoginDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
