using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Entities.Entities
{
    public class RelationShipRequest
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; } 
        public string? SenderId { get; set; }
        public virtual CustomIdentityUser? Sender { get; set; }
        public string? ReceiverId { get; set; }
    }
}
