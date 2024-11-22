using EventManagement.Models.ModelsDto.EventDateDtos;

namespace EventManagement.Models.ModelsDto.TicketDtos
{
    public class TicketDto
    {
        public string IdTicket { get; set; }
        public string EventId { get; set; }
        public string EventDateId { get; set; }
        public EventDateDto EventDate { get; set; }
        public string NameTicket { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string SaleStartDate { get; set; }
        public string SaleEndDate { get; set; }
        public string Description { get; set; }
        // Enum for Status
        public string Status { get; set; }

        // Enum for Visibility
        public string Visibility { get; set; }

        // Enum for Sale Method
        public string SaleMethod { get; set; }

    }
}
