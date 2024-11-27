using EventManagement.Data.Models.ChatRoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository.IRepository
{
    public interface IMessageRepository : IRepository<Message>
    {
        void Update(Message message);
    }
}
