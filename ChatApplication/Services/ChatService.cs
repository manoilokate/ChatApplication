using ChatApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Services
{
    public class ChatService
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Return the history of chat that is saved in db and order by date
        public async Task<List<ChatMessage>> GetChatHistoryAsync() 
        {
            return await _dbContext.ChatMessages
                .OrderByDescending(m => m.Date)
                .ToListAsync();
        }

        // Add message to db
        public async Task AddMessageAsync(ChatMessage message) 
        {
            _dbContext.ChatMessages.Add(message);
            await _dbContext.SaveChangesAsync();
        }
    }
}
