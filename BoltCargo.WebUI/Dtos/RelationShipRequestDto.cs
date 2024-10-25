using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class RelationShipRequestDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
        public string? SenderId { get; set; }
        public CustomIdentityUser? Sender { get; set; }
        public string? ReceiverId { get; set; }
    }
}
