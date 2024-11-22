using EventManagement.Data.Models;

namespace EventManagement.Models.ModelsDto.PurchasedDtos
{
    public class PurchasedTicketDto
    {
        public string IdPurchasedTicket { get; set; }
        public string OrderDetailId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
    }
}
