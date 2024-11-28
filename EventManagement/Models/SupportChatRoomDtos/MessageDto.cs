using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Models.ChatRoom
{
    public class MessageDto
    {
        public string MessageId { get; set; }
        public string SupportChatRoomId { get; set; }
        public string SenderId { get; set; }
        public bool IsSupport { get; set; }
        public string Content { get; set; }
        public string SendAt { get; set; }
    }
}
