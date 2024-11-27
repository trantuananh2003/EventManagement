namespace EventManagement.Models.SupportChatRoomDtos
{
    public class SendMessageDto
    {
        public string SenderId { get; set; }
        public string RoomId { get; set; }
        public string Content { get; set; }
        public bool IsSupport { get; set; }
    }
}
