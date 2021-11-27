using TestChatServer.Data.Entity;

namespace TestChatServer.DTO
{
    public class UserMessageWithoutIconDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }

        public UserMessageWithoutIconDTO()
        {

        }

        public UserMessageWithoutIconDTO(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }
    }
}
