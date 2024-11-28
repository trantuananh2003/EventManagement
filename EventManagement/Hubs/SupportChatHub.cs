using Microsoft.AspNetCore.SignalR;

namespace EventManagement.Hubs
{
    public class SupportChatHub : Hub
    {
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
    }
}
