namespace EventManagement.Models.SupportChatRoomDtos
{
    public class SendCreateChatRoomDto
    {
        public string OrganizationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
    }
}
