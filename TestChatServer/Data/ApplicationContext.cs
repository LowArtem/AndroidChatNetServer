using Microsoft.EntityFrameworkCore;
using TestChatServer.Data.Entity;

namespace TestChatServer.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }

        public ApplicationContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=chatappdb;Trusted_Connection=True;");
            string username = System.Environment.GetEnvironmentVariable("USER");
            string password = System.Environment.GetEnvironmentVariable("PASSWORD");

            optionsBuilder.UseSqlServer($"Server=sql303.epizy.com:3306;Database=epiz_30172183_chatdb;user={username};password={password}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Chat>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
