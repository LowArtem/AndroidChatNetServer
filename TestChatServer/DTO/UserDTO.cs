using System;
using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }
        public int Icon { get; set; }
        public string Username { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastDate { get; set; }
        
        public UserDTO(User user)
        {
            this.Id = user.Id;
            this.Icon = user.Icon;
            this.Username = user.Username;
            this.IsOnline = user.IsOnline;
            this.LastDate = user.LastDate;
        }
    }
}
