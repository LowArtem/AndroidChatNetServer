using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class UserMessageDTO
    {
        public long Id { get; set; }
        public int Icon { get; set; }
        public string Username { get; set; }

        public UserMessageDTO()
        {

        }

        public UserMessageDTO(User user)
        {
            this.Id = user.Id;
            this.Icon = user.Icon;
            this.Username = user.Username;
        }
    }
}
