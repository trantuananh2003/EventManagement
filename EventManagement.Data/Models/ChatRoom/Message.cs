namespace EventManagement.Data.Models.ChatRoom
{
    public class Message
    {
        public string MessageId { get; set; }
        public string SupportChatRoomId { get; set; }
        public SupportChatRoom SupportChatRoom { get; set; }

        public string SenderId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsSupport { get; set; }
        public string Content { get; set; }
    }
}
