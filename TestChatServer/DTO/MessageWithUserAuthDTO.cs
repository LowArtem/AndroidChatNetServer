using System;

namespace TestChatServer.DTO
{
    public class MessageWithUserAuthDTO
    {
        public string Text { get; set; }
        public UserAuthDTO Author { get; set; }
        public DateTime PubDate { get; set; }

        public MessageWithUserAuthDTO()
        {

        }
    }
}
