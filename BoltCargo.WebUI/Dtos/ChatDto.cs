using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class ChatDto
    {
        public int Id { get; set; }
        public string? ReceiverId { get; set; }
        public CustomIdentityUser? Receiver { get; set; }
        public string? SenderId { get; set; }
        public List<Message>? Messages { get; set; }
    }
}
