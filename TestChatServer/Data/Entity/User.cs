using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestChatServer.Data.Entity
{
    public class User : Entity
    {
        public int Icon { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastDate { get; set; }

        public List<Chat> Chats { get; set; }

        public User()
        {

        }

        public User(int icon, string username, string password, bool isOnline, DateTime lastDate)
        {
            Icon = icon;
            Username = username;
            Password = password;
            IsOnline = isOnline;
            LastDate = lastDate;
            Chats = new List<Chat>();
        }
    }
}
