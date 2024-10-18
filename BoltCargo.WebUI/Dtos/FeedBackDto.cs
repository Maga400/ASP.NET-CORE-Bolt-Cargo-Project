using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class FeedBackDto
    { 
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public string? UserId { get; set; }
        //public virtual CustomIdentityUser? User { get; set; }
    }
}
