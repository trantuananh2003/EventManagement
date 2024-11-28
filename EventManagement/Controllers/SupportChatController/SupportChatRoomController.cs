using EventManagement.Data.Models.ChatRoom;
using EventManagement.Hubs;
using EventManagement.Models;
using EventManagement.Models.SupportChatRoomDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace EventManagement.Controllers.SupportChatController
{
    [ApiController]
    [Route("api")]
    public class SupportChatRoomController : Controller
    {
        private readonly ApiResponse _apiResponse;
        private readonly ISupportChatService _supportChatService;
        private readonly IHubContext<SupportChatHub> _supportChatHub;

        public SupportChatRoomController(ISupportChatService supportChatService, IHubContext<SupportChatHub> supportChatHub)
        {
            _apiResponse = new ApiResponse();
            _supportChatService = supportChatService;
            _supportChatHub = supportChatHub;
        }

        [HttpGet("organization/{organizationId}/[Controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllChatRoomByOrganizationId([FromRoute]string organizationId)
        {
            var listDto = await _supportChatService.GetChatRoomsByOrganizationId(organizationId);

            _apiResponse.Result = listDto;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpGet("user/{userId}/[Controller]s")]
        public async Task<ActionResult<ApiResponse>> GetAllChatRoomByUserId([FromRoute] string userId)
        {
            var listDto = await _supportChatService.GetChatRoomsByUserId(userId);

            _apiResponse.Result = listDto;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPost("[Controller]")]
        public async Task<ActionResult<ApiResponse>> CreateSupportChatRoom([FromBody] SendCreateChatRoomDto sendCreateChatRoom)
        {
            await _supportChatService.CreateSupportChatRoom(sendCreateChatRoom.OrganizationId, sendCreateChatRoom.SenderId, sendCreateChatRoom.Content, sendCreateChatRoom.IsSupport);
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpPost("message")]
        public async Task<ActionResult<ApiResponse>> SendMessage([FromBody]SendMessageDto sendMessage)
        {
            var messageDto = await _supportChatService.SendMessage(sendMessage.SenderId, sendMessage.RoomId, sendMessage.Content, sendMessage.IsSupport);
            await _supportChatHub.Clients.Group(sendMessage.RoomId.ToString()).SendAsync("ReceiveMessage", messageDto);
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpGet("messages")]
        public async Task<ActionResult<ApiResponse>> FetchMessage([FromQuery] string chatRoomId)
        {
            var listDto = await _supportChatService.GetMessages(chatRoomId);
            _apiResponse.Result = listDto;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

    }
}
