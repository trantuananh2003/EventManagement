namespace EventManagement.Models.ModelsDto.OrderHeaderDtos
{
    public class OrderDataConfirm
    {
        public string StripePaymentIntentId { get; set; }
        public string Status { get; set; }
    }
}
