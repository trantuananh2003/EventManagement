using EventManagement.Data.DataConnect;
using EventManagement.Data.Models.ChatRoom;
using EventManagement.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Data.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private readonly ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext db):base(db) {
            _db = db;
        }

        public void Update(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
