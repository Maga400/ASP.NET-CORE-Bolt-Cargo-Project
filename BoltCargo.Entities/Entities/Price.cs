using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoltCargo.Entities.Entities
{
    public class Price
    {
        public int Id { get; set; }
        public string? VehicleName { get; set; }
        public double RoadPrice { get; set; }
        public int CarPrice { get; set; }
    }
}
