using System.Collections.Generic;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;
using TestChatServer.DTO;
using TestChatServer.Services;

namespace TestChatServer.BL
{
    public class MessageBL
    {
        private readonly MessageService messageService;
        private readonly UserService userService;
        private readonly ChatService chatService;

        public MessageBL(MessageService messageService, UserService userService, ChatService chatService)
        {
            this.messageService = messageService;
            this.userService = userService;
            this.chatService = chatService;
        }

        public async Task<bool> Save(long chatId, MessageWithUserAuthDTO messageDTO)
        {
            var chat = await chatService.GetChat(chatId);
            var user = await userService.GetUser(messageDTO.Author.Id);

            if (chat == null || user.Username != messageDTO.Author.Username || user.Password != messageDTO.Author.Password) return false;

            Message message = new Message(messageDTO.Text, user, messageDTO.PubDate, chat);

            return await messageService.SaveMessage(message);
        }

        public async Task<List<MessageDTO>> GetAllMessagesByChat(long chatId)
        {
            var chat = await chatService.GetChat(chatId);
            if (chat == null) return null;

            List<MessageDTO> messages = new List<MessageDTO>();
            foreach (var message in messageService.GetAllMessagesByChat(chatId))
            {
                messages.Add(new MessageDTO(message));
            }

            return messages;
        }

        public List<MessageDTO> GetAllMessagesByUser(long userId)
        {
            List<MessageDTO> messages = new List<MessageDTO>();

            if (userService.GetUser(userId).Result == null) return messages;

            foreach (var message in messageService.GetMessagesByUser(userId))
            {
                messages.Add(new MessageDTO(message));
            }

            return messages;
        }

        public async Task<List<MessageDTO>> GetPageOfMessages(long chatId, int messagesCount, int startIndex)
        {
            var chat = await chatService.GetChat(chatId);
            if (chat == null) return null;

            int totalMessagesCount = ((int)messageService.GetChatMessagesCount(chatId));
            if (startIndex > totalMessagesCount - 1) return new List<MessageDTO>();

            if (startIndex + messagesCount > totalMessagesCount)
            {
                var messages = messageService.GetNMessagesByIndexInChat(chatId, totalMessagesCount - startIndex, startIndex);

                var list = ConvertListToDTO(messages);
                list.Reverse();
                return list;
            }
            else
            {
                var messages = messageService.GetNMessagesByIndexInChat(chatId, messagesCount, startIndex);

                var list = ConvertListToDTO(messages);
                list.Reverse();
                return list;
            }
        }

        // false -> error
        public async Task<bool> DeleteMessageFromChat(long chatId, long messageId, long currentUserId)
        {
            var chat = await chatService.GetChat(chatId);
            if (chat == null) return false;

            var message = await messageService.GetMessage(messageId);
            if (message == null || message.Chat.Id != chatId) return false;

            // Deleting messages from dialogs is forbidden (before I make self-messages deleting)
            if (chat.SecondDialogMemberId != -1) return false;

            // Deleting creator's messages is forbidden (before I make self-messages deleting)
            if (message.Author.Id == chat.CreatorId || message.Author.Id == chat.SecondDialogMemberId) return false;

            // Admin's message can be deleted only by a creator
            if (chat.GetListOfAdminIds().Contains(message.Author.Id))
            {
                if (chat.CreatorId != currentUserId) return false;
            }

            await messageService.DeleteMessage(messageId);
            return true;
        }

        private static List<MessageDTO> ConvertListToDTO(List<Message> messages)
        {
            List<MessageDTO> messagesDTO = new List<MessageDTO>();
            foreach (var message in messages)
            {
                messagesDTO.Add(new MessageDTO(message));
            }

            return messagesDTO;
        }
    }
}
