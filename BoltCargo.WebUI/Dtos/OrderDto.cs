using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class OrderDto
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
        public bool IsAccept { get; set; }
        public decimal? RoadPrice { get; set; }
        public decimal? CarPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? UserId { get; set; }
        //public CustomIdentityUser? User { get; set; }
        public string? DriverId { get; set; }

    }
}
