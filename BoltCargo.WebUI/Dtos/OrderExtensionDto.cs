using BoltCargo.Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace BoltCargo.WebUI.Dtos
{
    public class OrderExtensionDto
    {
        [Required]
        public double Km { get; set; }
        [Required]
        public string? CarType { get; set; }
        [Required]
        public string? CurrentLocation { get; set; }
        [Required]
        public string? Destination { get; set; }
        //[Required]
        public string? ImagePath { get; set; }
        [Required]
        public string? Message { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        //public DateTime OrderAcceptedDate { get; set; }
        //public bool IsAccept { get; set; }
        [Required]
        public decimal? RoadPrice { get; set; }
        [Required]
        public decimal? CarPrice { get; set; }
        [Required]
        public decimal? TotalPrice { get; set; }
        [Required]
        public string? UserId { get; set; }
        //public UserDto? UserDto { get; set; }
        //public string? DriverId { get; set; }
    }
}
