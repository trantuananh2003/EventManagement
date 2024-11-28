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
        public Task<List<MessageDto>> GetMessages(string roomId);

        public Task CreateSupportChatRoom(string organizationId, string senderId, string content, Boolean isSupport);
        public Task<MessageDto> SendMessage(string senderId, string roomId, string content, bool isSupport);
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

        public  async Task<List<SupportChatRoomDto>> GetChatRoomsByOrganizationId(string OrganizationId)
        {
            var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.OrganizationId == OrganizationId, includeProperties: "User");
            var listDto = _mapper.Map<List<SupportChatRoomDto>>(listEntity);

            return listDto;
        }

        public async Task<List<SupportChatRoomDto>> GetChatRoomsByUserId(string UserId)
        {
            var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.UserId == UserId, includeProperties: "Organization");
            var listDto = _mapper.Map<List<SupportChatRoomDto>>(listEntity);

            return listDto;
        }

        public async Task CreateSupportChatRoom(string organizationId, string senderId, string content, bool isSupport=false)
        {
            var user = await _userManager.FindByIdAsync(senderId);

            var supportChatRoom = new SupportChatRoom() {
                SupportChatRoomId = Guid.NewGuid().ToString(),
                OrganizationId = organizationId,
                UserId = user.Id,
                Name = user.FullName,
            };

            var message = new Message() {
                MessageId = Guid.NewGuid().ToString(),
                Content = content,
                IsSupport = isSupport,
                SenderId = senderId,
                SupportChatRoomId = supportChatRoom.SupportChatRoomId,
            };

            await _dbSupportChatRoom.CreateAsync(supportChatRoom);
            await _dbMessage.CreateAsync(message);
            await _dbSupportChatRoom.SaveAsync();
        }

        public async Task<MessageDto> SendMessage(string senderId, string roomId ,string content,bool isSupport)
        {
            var chatRoom = await _dbSupportChatRoom.GetAsync(x => x.SupportChatRoomId == roomId);
            var message = new Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                SupportChatRoomId = chatRoom.SupportChatRoomId,
                Content = content,
                IsSupport = isSupport,
                SenderId = senderId,
                SendAt = DateTime.Now,
            };

            await _dbMessage.CreateAsync(message);
            await _dbMessage.SaveAsync();

            var messageDto = new MessageDto()
            {
                MessageId = message.MessageId,
                IsSupport = isSupport,
                SenderId = senderId,
                SupportChatRoomId = roomId,
                Content = content,
                SendAt =  message.SendAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return messageDto;
        }

        public async Task<List<MessageDto>> GetMessages(string roomId)
        {
            var listEntity = await _dbMessage.GetAllAsync(x => x.SupportChatRoomId == roomId);
            var listDto = _mapper.Map<List<MessageDto>>(listEntity);

            // Sắp xếp theo SendAt (cũ đến mới)
            listDto = listDto.OrderBy(x => x.SendAt).ToList(); 

            return listDto;
        }

    }
}
