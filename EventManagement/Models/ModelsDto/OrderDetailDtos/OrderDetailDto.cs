using EventManagement.Models.ModelsDto.TicketDtos;

namespace EventManagement.Models.ModelsDto.OrderDetailDtos
{
    public class OrderDetailDto
    {
        public string TicketId { get; set; }
        public TicketDto Ticket { get; set; }
        public int Quantity { get; set; }
    }
}
