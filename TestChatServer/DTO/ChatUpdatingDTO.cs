using System.Collections.Generic;
using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class ChatUpdatingDTO
    {
        public long ChatId { get; set; }
        public string Name { get; set; }
        public string About { get; set; } = null;
        public int Icon { get; set; }
        public long CreatorId { get; set; }

        public ChatUpdatingDTO()
        {

        }

        public ChatUpdatingDTO(Chat chat)
        {
            this.ChatId = chat.Id;
            this.Name = chat.Name;
            this.About = chat.About;
            this.Icon = chat.Icon;
            this.CreatorId = chat.CreatorId;
        }
    }
}
