using EventManagement.Models.SupportChatRoomDtos;
using EventManagement.Models;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EventManagement.Hubs
{
    public class SupportChatHub : Hub
    {
        private readonly ISupportChatService _supportChatService;

        public SupportChatHub(ISupportChatService supportChatService)
        {
            _supportChatService = supportChatService;
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            Console.WriteLine($"User {Context.ConnectionId} joined room {roomId}");
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            Console.WriteLine($"User {Context.ConnectionId} left room {roomId}");
        }

        public async Task SendMessage(SendMessageDto sendMessage)
        {
            var messageDto = await _supportChatService.SendMessage(
                sendMessage.SenderId,
                sendMessage.RoomId,
                sendMessage.Content,
                sendMessage.IsSupport
            );

            await Clients.Group(sendMessage.RoomId).SendAsync("ReceiveMessage", messageDto);

            var chatRoom = await _supportChatService.GetChatRoomById(sendMessage.RoomId);
            await Clients.Group(sendMessage.RoomId).SendAsync("ReceiveChatRoom", chatRoom);
        }

        public async Task MarkAsMessage(string roomId)
        {
            await _supportChatService.MarkRead(roomId, true);
        }
    }
}
