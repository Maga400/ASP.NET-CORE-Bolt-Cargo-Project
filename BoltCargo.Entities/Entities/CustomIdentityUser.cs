using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Entities.Entities
{
    public class CustomIdentityUser : IdentityUser
    {
        public string? CarType { get; set; }
        public string? ImagePath { get; set; }
        public bool IsOnline { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<FeedBack>? FeedBacks { get; set; }
        public CustomIdentityUser()
        {
            Orders = new List<Order>();
            FeedBacks = new List<FeedBack>();
        }
    }
}
