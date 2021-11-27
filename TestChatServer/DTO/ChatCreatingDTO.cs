using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class ChatCreatingDTO
    {
        public string Name { get; set; }
        public int Icon { get; set; }
        public long CreatorId { get; set; }

        public string About { get; set; } = null;

        public ChatCreatingDTO()
        {

        }

        public ChatCreatingDTO(Chat chat)
        {
            this.Name = chat.Name;
            this.Icon = chat.Icon;
            this.CreatorId = chat.CreatorId;
            this.About = chat.About;
        }
    }
}
