namespace BoltCargo.WebUI.Dtos
{
    public class ComplaintAddDto
    {
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime Date { get; set; }
    }
}
