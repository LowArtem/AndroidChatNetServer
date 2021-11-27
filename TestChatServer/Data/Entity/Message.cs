using System;
using System.ComponentModel.DataAnnotations;

namespace TestChatServer.Data.Entity
{
    public class Message : Entity
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public DateTime PubDate { get; set; }

        public Chat Chat { get; set; }

        public Message()
        {

        }

        public Message(string text, User author, DateTime pubDate, Chat chat)
        {
            Text = text;
            Author = author;
            PubDate = pubDate;
            Chat = chat;
        }
    }
}
