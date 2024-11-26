using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? CarType { get; set; }
        public string? ImagePath { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
