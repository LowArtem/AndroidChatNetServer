using TestChatServer.Data.Entity;
using TestChatServer.Services;

namespace TestChatServer.DTO
{
    public class ChatInfoDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Icon { get; set; }
        public int SecondIcon { get; set; }
        public bool isDialog { get; set; } = false;

        public ChatInfoDTO(Chat chat, UserService userService)
        {
            this.Id = chat.Id;
            this.Name = chat.Name;
            this.Icon = chat.Icon;

            if (chat.SecondDialogMemberId != -1)
            {
                this.isDialog = true;

                var secondMember = userService.GetUser(chat.SecondDialogMemberId).Result;
                if (secondMember == null)
                {
                    this.SecondIcon = -1;
                }
                else
                {
                    this.SecondIcon = secondMember.Icon;
                }
            }
            else
            {
                this.isDialog = false;
                this.SecondIcon = -1;
            }
        }
    }    
}
