using EventManagement.Common;
using EventManagement.Data.Models.ChatRoom;
using EventManagement.Hubs;
using EventManagement.Models;
using EventManagement.Models.SupportChatRoomDtos;
using EventManagement.Service;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = SD_Role_Permission.SupportChat_ClaimValue)]
        public async Task<ActionResult<ApiResponse>> GetAllChatRoomByOrganizationId([FromRoute]string organizationId)
        {
            var listDto = await _supportChatService.GetChatRoomsByOrganizationId(organizationId);

            _apiResponse.Result = listDto;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }


        [HttpGet("[controller]")]
        public async Task<ActionResult<ApiResponse>> GetChatRoomByOrganizationId([FromQuery] string organizationId,[FromQuery]string senderId)
            {
            var entityDto = await _supportChatService.GetChatRoomByOrganizationId(organizationId, senderId);

            if(entityDto == null)
            {
                _apiResponse.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_apiResponse);
            }

            _apiResponse.Result = entityDto;
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

        [HttpGet("messages")]
        public async Task<ActionResult<ApiResponse>> FetchMessage([FromQuery] string chatRoomId)
        {
            var listDto = await _supportChatService.GetMessages(chatRoomId);
            _apiResponse.Result = listDto;
            _apiResponse.IsSuccess = true;
            return Ok(_apiResponse);
        }

        [HttpPatch("[Controller]/markread")]
        public async Task<ActionResult<ApiResponse>> MarkAsRead(string roomId, bool isUser)
        {
            await _supportChatService.MarkRead(roomId, isUser);
            return Ok(_apiResponse);
        }

        [HttpPost("[Controller]")]
        public async Task<ActionResult<ApiResponse>> CreateSupportChatRoom([FromForm] SendCreateChatRoomDto sendCreateChatRoom)
        {
            var roomId = await _supportChatService.CreateSupportChatRoom(sendCreateChatRoom.OrganizationId, sendCreateChatRoom.SenderId);
            //await _supportChatService.SendMessage(sendCreateChatRoom.SenderId, roomId, sendCreateChatRoom.Content, false);
            var room = await _supportChatService.GetChatRoomById(roomId);
            _apiResponse.Result = room;
            _apiResponse.IsSuccess = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            return Ok(_apiResponse);
        }



    }
}
