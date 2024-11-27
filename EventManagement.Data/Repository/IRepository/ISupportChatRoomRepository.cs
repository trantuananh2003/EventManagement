using EventManagement.Data.Models;
using EventManagement.Data.Models.ChatRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository.IRepository
{
    public interface ISupportChatRoomRepository : IRepository<SupportChatRoom>
    {
        void Update(SupportChatRoom entity);
    }
}
