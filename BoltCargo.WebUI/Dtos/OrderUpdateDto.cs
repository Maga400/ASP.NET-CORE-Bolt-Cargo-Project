using BoltCargo.Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace BoltCargo.WebUI.Dtos
{
    public class OrderUpdateDto
    {
        //[Required]
        //public int Id { get; set; }
        [Required]
        public DateTime OrderAcceptedDate { get; set; }
        [Required]
        public bool IsAccept { get; set; }
        [Required]
        public string? DriverId { get; set; }
        //public CustomIdentityUser? Driver { get; set; }
    }
}
