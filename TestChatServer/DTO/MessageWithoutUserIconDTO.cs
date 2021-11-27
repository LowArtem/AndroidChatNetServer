using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class MessageWithoutUserIconDTO
    {
        public string Text { get; set; }
        public UserMessageWithoutIconDTO Author { get; set; }
        public DateTime PubDate { get; set; }

        public MessageWithoutUserIconDTO()
        {

        }

        public MessageWithoutUserIconDTO(Message message)
        {
            this.Text = message.Text;
            this.Author = new UserMessageWithoutIconDTO(message.Author);
            this.PubDate = message.PubDate;
        }
    }
}
