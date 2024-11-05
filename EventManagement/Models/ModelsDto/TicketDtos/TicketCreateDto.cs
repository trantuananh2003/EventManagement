using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models.ModelsDto.TicketDtos
{
    public class TicketCreateDto
    {
        [Required]
        public string EventId { get; set; }
        [Required]
        public string EventDateId { get; set; }
        public string NameTicket { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        [Required]
        public DateTime SaleStartDate { get; set; }
        [Required]
        public DateTime SaleEndDate { get; set; }
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Visibility { get; set; }
        [Required]
        public string SaleMethod { get; set; }
    }
}
