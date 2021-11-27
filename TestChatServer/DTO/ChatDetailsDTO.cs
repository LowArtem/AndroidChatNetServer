using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;
using TestChatServer.Services;

namespace TestChatServer.DTO
{
    public class ChatDetailsDTO
    {
        public string Name { get; set; }
        public string About { get; set; }
        public int Icon { get; set; }
        public int SecondIcon { get; set; } = -1;
        public long CreatorId { get; set; }
        public long SecondDialogMemberId { get; set; } = -1;

        public ChatDetailsDTO(Chat chat, UserService userService)
        {
            this.Name = chat.Name;
            this.About = chat.About;
            this.Icon = chat.Icon;
            this.CreatorId = chat.CreatorId;
            this.SecondDialogMemberId = chat.SecondDialogMemberId;

            // TODO: не работает, пофиксить secondIcon
            if (chat.SecondDialogMemberId != -1)
            {
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
                this.SecondIcon = -1;
            }
        }
    }
}
