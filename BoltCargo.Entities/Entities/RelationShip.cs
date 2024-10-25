using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Entities.Entities
{
    public class RelationShip
    {
        public int Id { get; set; }
        public string? OwnId { get; set; }
        public string? YourRelationShipId { get; set; }
        public virtual CustomIdentityUser? YourRelationShip { get; set; }
        public DateTime? RelationShipDate { get; set; }
    }
}
