using EventManagement.Data.Models;
using EventManagement.Models.ModelsDto.OrderDetailDtos;

namespace EventManagement.Models.ModelsDto.OrderHeaderDtos
{
    public class OrderHeaderDto
    {
        public string IdOrderHeader { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string NumberPhone { get; set; }
        public string OrderDate { get; set; }
        public int PriceTotal { get; set; } 
        public string Status { get; set; }
        public int TotalItem { get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
