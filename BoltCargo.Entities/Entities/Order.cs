using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Entities.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Km { get; set; }
        public string? CarType { get; set; }
        public string? CurrentLocation { get; set; }
        public string? Destination { get; set; }
        public string? ImagePath { get; set; }
        public string? Message { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderAcceptedDate { get; set; }
        public bool IsAccept { get; set; } = false;
        public bool IsFinish { get; set; }
        public bool IsClientFinish { get; set; }
        public bool IsDriverFinish { get; set; }
        public decimal? RoadPrice { get; set; }
        public decimal? CarPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? UserId { get; set; }
        public virtual CustomIdentityUser? User { get; set; }
        public string? DriverId { get; set; }
        //public virtual CustomIdentityUser? Driver { get; set; }

    }
}
