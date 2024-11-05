namespace EventManagement.Models.ModelsDto.TicketDtos
{
    public class TicketUpdateDto
    {
        public string IdTicket { get; set; }
        public string EventId { get; set; }
        public string EventDateId { get; set; }
        public string NameTicket { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string SaleStartDate { get; set; }
        public string SaleEndDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Visibility { get; set; }
        public string SaleMethod { get; set; }
    }
}
