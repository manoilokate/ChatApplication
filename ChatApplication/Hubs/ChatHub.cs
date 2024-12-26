using ChatApplication.Data;
using ChatApplication.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        // Return the history of chat
        public async Task<List<ChatMessage>> GetChatHistory() 
        {
            return await _chatService.GetChatHistoryAsync();
        }

        // Send message to chat
        public async Task SendMessage(string userName, string message) 
        {
            var chatMessage = new ChatMessage
            {
                UserName = userName,
                Message = message,
                Date = DateTime.Now
            };
            await _chatService.AddMessageAsync(chatMessage);
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
    }
}
