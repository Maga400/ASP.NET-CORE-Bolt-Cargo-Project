namespace BoltCargo.WebUI.Dtos
{
    public class CardDto
    {
        public int Id { get; set; }
        public string? BankName { get; set; }
        public string? CardNumber { get; set; }
        public decimal? Balance { get; set; }
        public string? UserId { get; set; }

    }
}
