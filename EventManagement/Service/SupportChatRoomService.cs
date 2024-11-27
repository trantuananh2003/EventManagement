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
        public Task CreateSupportChatRoom(string organizationId, string senderId, string content, Boolean isSupport);
        public Task SendMessage(string senderId, string roomId, string content, bool isSupport);
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
            var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.OrganizationId == OrganizationId);
            var listDto = _mapper.Map<List<SupportChatRoomDto>>(listEntity);

            return listDto;
        }

        public async Task<List<SupportChatRoomDto>> GetChatRoomsByUserId(string UserId)
        {
            var listEntity = await _dbSupportChatRoom.GetAllAsync(x => x.UserId == UserId);
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

        public async Task SendMessage(string senderId, string roomId ,string content,bool isSupport)
        {
            var charRoom = await _dbSupportChatRoom.GetAsync(x => x.SupportChatRoomId == roomId);
            var message = new Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                SupportChatRoomId = charRoom.SupportChatRoomId,
                Content = content,
                IsSupport = isSupport,
                SenderId = senderId,
            };

            await _dbMessage.CreateAsync(message);
            await _dbMessage.SaveAsync();
        }
    }
}
