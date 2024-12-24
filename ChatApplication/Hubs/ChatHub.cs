using ChatApplication.Data;
using ChatApplication.Services;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    private readonly ChatService _chatService;

    public ChatHub(ChatService chatService)
    {
        _chatService = chatService;
    }

    public async Task<List<ChatMessage>> GetChatHistory()
    {
        return await _chatService.GetChatHistoryAsync();
    }

    public async Task SendMessage(string user, string message)
    {
        var chatMessage = new ChatMessage
        {
            UserName = user,
            Message = message,
            Date = DateTime.Now
        };

        await _chatService.AddMessageAsync(chatMessage);
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
}
