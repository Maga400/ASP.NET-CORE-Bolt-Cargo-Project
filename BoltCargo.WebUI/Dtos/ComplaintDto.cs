using BoltCargo.Entities.Entities;

namespace BoltCargo.WebUI.Dtos
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public string? SenderId { get; set; }
        public CustomIdentityUser? Sender {  get; set; }
        public string? ReceiverId { get; set; }
        public CustomIdentityUser? Receiver { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
    }
}
