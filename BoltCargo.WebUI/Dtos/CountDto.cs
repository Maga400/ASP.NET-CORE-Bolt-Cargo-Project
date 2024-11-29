namespace BoltCargo.WebUI.Dtos
{
    public class CountDto
    {
        public int AllUsersCount { get; set; }
        public int AdminsCount { get; set; }
        public int DriversCount { get; set; }
        public int ClientsCount { get; set; }
        public int AllOrdersCount { get; set; }
        public int AcceptedOrdersCount { get; set; }
        public int UnAcceptedOrdersCount { get; set; }
        public int FinishedOrdersCount { get; set; }
        public int AllComplaintsCount { get; set; }
    }
}
