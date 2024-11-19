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
        public bool IsBan { get; set; }
        public bool IsOnline { get; set; }
        public bool IsRelationShip {  get; set; }
        public bool HasRequestPending { get; set; }
        public DateTime DisConnectTime { get; set; } = DateTime.Now;
        public string? ConnectTime { get; set; } = "";
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<FeedBack>? FeedBacks { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ICollection<RelationShip> RelationShips { get; set; }
        public virtual ICollection<RelationShipRequest> RelationShipsRequest { get; set; }
        public CustomIdentityUser()
        {
            Orders = new List<Order>();
            FeedBacks = new List<FeedBack>();
            Chats = new List<Chat>();
            RelationShips = new List<RelationShip>();
            RelationShipsRequest = new List<RelationShipRequest>();
        }
    }
}
