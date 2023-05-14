using ChatSignalRAzurePost.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalRAzurePost.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

        public DbSet<ChatPost> Posts { get; set; }
    }
}
