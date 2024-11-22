using EventManagement.Data.Models;

namespace EventManagement.Models.ModelsDto.OrderDetailDtos
{
    public class OrderDetailCreateDto
    {
        public string TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
