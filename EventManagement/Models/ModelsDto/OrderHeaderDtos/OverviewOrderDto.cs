using EventManagement.Models.ModelsDto.EventDtos;

namespace EventManagement.Models.ModelsDto.OrderHeaderDtos
{
    public class OverviewOrderDto
    {
        public string IdOrderHeader { get; set; }
        public string UrlImage { get; set; }
        public string NameEvent { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
        public string TotalTicket { get; set; }
        public string TotalPrice { get; set; }
    }


}
