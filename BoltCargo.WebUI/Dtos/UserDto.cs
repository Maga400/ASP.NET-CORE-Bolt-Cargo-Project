using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class UserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? ImagePath { get; set; }
        public string? CarType { get; set; }
        public string? Email { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<FeedBack>? FeedBacks { get; set; } 
    }
}
