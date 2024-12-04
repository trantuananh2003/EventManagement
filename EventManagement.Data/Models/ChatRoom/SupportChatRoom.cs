namespace EventManagement.Data.Models.ChatRoom
{
    public class SupportChatRoom
    {
        public string SupportChatRoomId {get;set;}
        public string OrganizationId { get; set; } //Tổ chức nhận tin nhắn
        public Organization Organization { get; set; }
        public string UserId { get; set; } //Người cần hỗ tr
        public ApplicationUser User { get; set; }
        public string Name { get; set; }
        public DateTime LastMessageTime { get; set; } //Sap xep hoi thoai
        public int IsReadFromUser { get; set; } 
        public int IsReadFromOrganizaiton { get; set; }
    }
}
