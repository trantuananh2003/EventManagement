namespace EventManagement.Data.Models.ChatRoom
{
    public class SupportChatRoom
    {
        public string SupportChatRoomId {get;set;}
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Name { get; set; }
    }
}
