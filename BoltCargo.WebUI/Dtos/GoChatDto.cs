using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class GoChatDto
    {
        public string? CurrentUserId { get; set; }
        public Chat? CurrentChat { get; set; }
        public IEnumerable<Chat>? Chats { get; set; }
        public string? CurrentReceiver { get; set; }
        public string? CurrentUserName { get; set; }
        public string? CurrentReceiverImage { get; set; }
    }
}
