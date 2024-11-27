using EventManagement.Data.DataConnect;
using EventManagement.Data.Models.ChatRoom;
using EventManagement.Data.Repository.IRepository;

namespace EventManagement.Data.Repository
{
    public class SupportChatRoomRepository : Repository<SupportChatRoom>, ISupportChatRoomRepository
    {
        private readonly ApplicationDbContext _db;

        public SupportChatRoomRepository(ApplicationDbContext db) :base(db) {
            _db = db;
        }

        public void Update(SupportChatRoom entity)
        {
            throw new NotImplementedException();
        }
    }
}
