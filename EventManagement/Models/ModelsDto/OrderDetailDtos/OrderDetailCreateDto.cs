using EventManagement.Data.Models;

namespace EventManagement.Models.ModelsDto.OrderDetailDtos
{
    public class OrderDetailCreateDto
    {
        public string IdOrderDetail { get; set; }
        public string OrderHeaderId { get; set; }
        public string TicketId { get; set; }
        public int Quantity { get; set; }
        public string NameTicket { get; set; }
        public int Price { get; set; }
    }
}
