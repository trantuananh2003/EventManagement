namespace EventManagement.Models.ModelsDto.OrderHeaderDtos
{
    public class OrderConfirmDto
    {
        public string OrderHeaderId { get; set; }
        public string StripePaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public int TotalPrice { get; set; }
        public string NumberPhone { get; set; }
        public string FullName { get; set; }
    }
}
