using EventManagement.Data.Models;
using EventManagement.Models.ModelsDto.OrderDetailDtos;

namespace EventManagement.Models.ModelsDto.OrderHeaderDtos
{
    public class OrderHeaderCreateDto
    {
        public string IdOrderHeader { get; set; }
        public string UserId { get; set; }
        public string NumberPhone { get; set; }
        public int PriceTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
        public int TotalItem { get; set; }
        public IEnumerable<OrderDetailCreateDto> OrderDetails { get; set; }
    }
}
