using AutoMapper;
using EventManagement.Data.Models;
using EventManagement.Data.Models.ChatRoom;
using EventManagement.Data.Repository.IRepository;
using EventManagement.Models.SupportChatRoomDtos;
using Microsoft.AspNetCore.Identity;

namespace EventManagement.Service
{
    public interface ISupportChatService
    {
        public Task<List<SupportChatRoomDto>> GetChatRoomsByOrganizationId(string OrganizationId);
        public Task<List<SupportChatRoomDto>> GetChatRoomsByUserId(string UserId);
        public Task<SupportChatRoomDto> GetChatRoomById(string roomId);
        public Task<SupportChatRoomDto> GetChatRoomByOrganizationId(string organizationId, string senderId);

        public Task<List<MessageDto>> GetMessages(string roomId);

        public Task<string> CreateSupportChatRoom(string organizationId, string senderId);
        public Task<MessageDto> SendMessage(string senderId, string roomId, string content, bool isSupport);
        public Task MarkRead(string roomId,bool isUser);
        public Task<int> GetUnreadMessagesCount(string id, bool isUser);
    }

    public class SupportChatService : ISupportChatService
    {
        private ISupportChatRoomRepository _dbSupportChatRoom;
        private IMessageRepository _dbMessage;
        private UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;

        public SupportChatService(ISupportChatRoomRepository dbSupportChatRoom, IMessageRepository dbMessage, IMapper mapper
            ,UserManager<ApplicationUser> userManager)
        {
            _dbSupportChatRoom = dbSupportChatRoom;
            _dbMessage = dbMessage;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<SupportChatRoomDto>> GetChatRoomsByOrganizationId(string OrganizationId)
        {
            var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.OrganizationId == OrganizationId, includeProperties: "User");
            var orderedListEntity = listEntity.OrderByDescending(c => c.LastMessageTime).ToList();
            var listDto = _mapper.Map<List<SupportChatRoomDto>>(orderedListEntity);

            return listDto;
        }

        public async Task<List<SupportChatRoomDto>> GetChatRoomsByUserId(string UserId)
        {
            var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.UserId == UserId, includeProperties: "Organization");
            var orderedListEntity = listEntity.OrderByDescending(c => c.LastMessageTime).ToList();
            var listDto = _mapper.Map<List<SupportChatRoomDto>>(orderedListEntity);

            return listDto;
        }

        public async Task<string> CreateSupportChatRoom(string organizationId, string senderId)
        {
            var user = await _userManager.FindByIdAsync(senderId);

            var chatRoom = await _dbSupportChatRoom.GetAsync(x => x.OrganizationId == organizationId && x.UserId == senderId);
            if(chatRoom == null)
            {
                var supportChatRoom = new SupportChatRoom()
                {
                    SupportChatRoomId = Guid.NewGuid().ToString(),
                    OrganizationId = organizationId,
                    UserId = user.Id,
                    Name = user.FullName,
                };
                await _dbSupportChatRoom.CreateAsync(supportChatRoom);
                await _dbSupportChatRoom.SaveAsync();
                return supportChatRoom.SupportChatRoomId;
            }
            return chatRoom.SupportChatRoomId;
        }

        public async Task<MessageDto> SendMessage(string senderId, string roomId, string content, bool isSupport)
        {
            // Lấy thông tin phòng chat theo roomId
            var chatRoom = await _dbSupportChatRoom.GetAsync(x => x.SupportChatRoomId == roomId, tracked: true);

            // Cập nhật trạng thái đọc cho tổ chức và người dùng
            chatRoom.IsReadFromOrganizaiton = 0;
            chatRoom.IsReadFromUser = 0;

            // Tạo mới tin nhắn
            var message = new Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                SupportChatRoomId = chatRoom.SupportChatRoomId,
                Content = content,
                IsSupport = isSupport,
                SenderId = senderId,
                SendAt = DateTime.Now,
                
            };

            // Lưu tin nhắn vào cơ sở dữ liệu
            await _dbMessage.CreateAsync(message);

            // Cập nhật thời gian của tin nhắn cuối cùng trong phòng chat
            chatRoom.LastMessageTime = DateTime.Now;

            // Lưu thông tin phòng chat đã cập nhật
            await _dbSupportChatRoom.SaveAsync();

            // Lưu các thay đổi tin nhắn
            await _dbMessage.SaveAsync();

            // Chuyển đổi tin nhắn thành DTO để trả về cho client
            var messageDto = new MessageDto()
            {
                MessageId = message.MessageId,
                IsSupport = isSupport,
                SenderId = senderId,
                SupportChatRoomId = roomId,
                Content = content,
                SendAt = message.SendAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return messageDto;
        }

        public async Task<List<MessageDto>> GetMessages(string roomId)
        {
            var listEntity = await _dbMessage.GetAllAsync(x => x.SupportChatRoomId == roomId);
            var listDto = _mapper.Map<List<MessageDto>>(listEntity);
            listDto = listDto.OrderBy(x => x.SendAt).ToList();
            return listDto;
        }

        public async Task MarkRead(string roomId, bool isUser)
        {
            var entity =  await _dbSupportChatRoom.GetAsync(x => x.SupportChatRoomId == roomId , tracked: true);
            if (isUser && entity != null)
            {
               entity.IsReadFromUser = 1;
            }
            else if (entity!=null)
            {
                entity.IsReadFromUser = 1;
            }
            await _dbSupportChatRoom.SaveAsync();
        }

        public async Task<int> GetUnreadMessagesCount(string id, bool isUser)
        {
            int unreadCount = 0;
            if (isUser)
            {
                var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.UserId == id && x.IsReadFromUser == 0);
                unreadCount = listEntity.Count();
            }
            else
            {
                var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.UserId == id && x.IsReadFromUser == 0);
                unreadCount = listEntity.Count();
            }
            return unreadCount;
        }

        public async Task<SupportChatRoomDto> GetChatRoomById(string roomId)
        {
            var entity = await _dbSupportChatRoom.GetAsync(x => x.SupportChatRoomId == roomId, includeProperties:"User,Organization");
            var modelDto = _mapper.Map<SupportChatRoomDto>(entity);
            return modelDto;
        }

        public async Task<SupportChatRoomDto> GetChatRoomByOrganizationId(string organizationId, string senderId)
        {
            var entity = await _dbSupportChatRoom.GetAsync(x => x.OrganizationId == organizationId && x.UserId == senderId);
            return _mapper.Map<SupportChatRoomDto>(entity);
        }

    }
}
