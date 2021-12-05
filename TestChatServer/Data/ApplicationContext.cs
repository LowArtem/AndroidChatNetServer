using Microsoft.EntityFrameworkCore;
using System;
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

            string pgUserId = Environment.GetEnvironmentVariable("POSTGRES_USER_ID");
            string pgPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
            string pgHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            string pgPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            string pgDatabase = Environment.GetEnvironmentVariable("POSTGRES_DB");

            string connStr = $"Server={pgHost};Port={pgPort};User Id={pgUserId};Password={pgPassword};Database={pgDatabase}";
            optionsBuilder.UseNpgsql(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Chat>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
