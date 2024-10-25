using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class RelationShipDto
    {
        public int Id { get; set; }
        public string? OwnId { get; set; }
        public string? YourRelationShipId { get; set; }
        public CustomIdentityUser? YourRelationShip { get; set; }
        public DateTime? RelationShipDate { get; set; }
    }
}
