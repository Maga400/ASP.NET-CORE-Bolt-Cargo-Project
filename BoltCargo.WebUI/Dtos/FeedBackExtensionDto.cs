using BoltCargo.Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace BoltCargo.WebUI.Dtos
{
    public class FeedBackExtensionDto
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? UserId { get; set; }
        public virtual CustomIdentityUser? User { get; set; }
    }
}
