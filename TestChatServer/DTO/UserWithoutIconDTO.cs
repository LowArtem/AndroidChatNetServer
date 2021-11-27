using System;
using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class UserWithoutIconDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastDate { get; set; }       

        public UserWithoutIconDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            IsOnline = user.IsOnline;
            LastDate = user.LastDate;
        }
    }
}
