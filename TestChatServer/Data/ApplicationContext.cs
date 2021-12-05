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

            string connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // Parse connection URL to connection string for Npgsql
            connUrl = connUrl.Replace("postgres://", string.Empty);

            string pgHostPortDb = connUrl.Split("@")[1];
            string pgUserPass = connUrl.Split("@")[0];
            string pgHostPort = pgHostPortDb.Split("/")[0];

            string pgDb = pgHostPortDb.Split("/")[1];
            string pgUser = pgUserPass.Split(":")[0];
            string pgPass = pgUserPass.Split(":")[1];
            string pgHost = pgHostPort.Split(":")[0];
            string pgPort = pgHostPort.Split(":")[1];

            string connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};Trust Server Certificate=True;";
            optionsBuilder.UseNpgsql(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Chat>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
