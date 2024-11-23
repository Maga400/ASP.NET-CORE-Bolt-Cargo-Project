using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Entities.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public string? BankName { get; set; }
        public string? CardNumber { get; set; }
        public double Balance { get; set; }
        public string? UserId { get; set; }
        public CustomIdentityUser? User { get; set; }

    }
}
