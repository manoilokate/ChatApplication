using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Data
{
    // Db that inheriting DbContext(options) 
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
