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
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            //optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=chat-test-server;Username=postgres;password=postgres");

            string connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            connUrl = connUrl.Replace("postgres://", string.Empty);

            string pgHostPortDb = connUrl.Split("@")[1];
            string pgUserPass = connUrl.Split("@")[0];
            string pgHostPort = pgHostPortDb.Split("/")[0];

            string pgDb = pgHostPortDb.Split("/")[1];
            string pgUser = pgUserPass.Split(":")[0];
            string pgPass = pgUserPass.Split(":")[1];
            string pgHost = pgHostPort.Split(":")[0];
            string pgPort = pgHostPort.Split(":")[1];

            string connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;Trust Server Certificate=True;";
            optionsBuilder.UseNpgsql(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Chat>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
