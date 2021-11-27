using System;
using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class MessageDTO
    {
        public string Text { get; set; }
        public UserMessageDTO Author { get; set; }
        public DateTime PubDate { get; set; }

        public MessageDTO()
        {

        }
        
        public MessageDTO(Message message)
        {
            this.Text = message.Text;
            this.Author = new UserMessageDTO(message.Author);
            this.PubDate = message.PubDate;
        }
    }
}
