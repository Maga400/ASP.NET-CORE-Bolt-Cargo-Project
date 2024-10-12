using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Entities.Entities
{
    public class FeedBack
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public string? UserId { get; set; }
        public virtual CustomIdentityUser? User { get; set; }
    }
}
