using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;
using TestChatServer.DTO;
using TestChatServer.Services;

namespace TestChatServer.BL
{
    public class ChatBL
    {
        private readonly ChatService chatService;
        private readonly UserService userService;
        private readonly ILogger logger;

        public ChatBL(ChatService chatService, UserService userService, ILogger<ChatBL> logger)
        {
            this.chatService = chatService;
            this.userService = userService;
            this.logger = logger;
        }

        public async Task<List<ChatInfoDTO>> GetAllChatInfoByUser(long userId)
        {
            var chats = await chatService.GetAllChatsByUser(userId);

            if (chats == null) return null;

            var chatsDTO = new List<ChatInfoDTO>();
            foreach (var chat in chats)
            {
                chatsDTO.Add(new ChatInfoDTO(chat, userService));
            }

            return chatsDTO;
        }

        public async Task<ChatDetailsDTO> GetChatById(long chatId)
        {
            var chat = await chatService.GetChat(chatId);
            if (chat == null) return null;

            return new ChatDetailsDTO(chat, userService);
        }

        public async Task<List<UserDTO>> GetChatMembers(long chatId)
        {
            var chat = await chatService.GetChat(chatId);
            if (chat == null) return null;

            var usersDTO = new List<UserDTO>();
            if (usersDTO == null) return null;

            foreach (var member in chat.Members)
            {
                usersDTO.Add(new UserDTO(member));
            }

            return usersDTO;
        }

        // return -1 => error
        public async Task<long> CreateChat(ChatCreatingDTO chatCreatingDTO)
        {
            var user = await userService.GetUser(chatCreatingDTO.CreatorId);
            if (user == null)
            {
                logger.LogWarning("CreateChat -> User is null");
                return -1;
            }

            // Dialog - системное название диалоговых чатов, запрещено для ввода как часть названия
            // не забыть сделать фильтр в клиенте
            if (chatCreatingDTO.Name.ToLower().Contains("dialog"))
            {
                logger.LogWarning("CreateChat -> trying to create chat with name dialog");
                return -1;
            }

            var foundedChat = chatService.GetChatByNameExactly(chatCreatingDTO.Name);
            if (foundedChat != null)
            {
                logger.LogWarning("CreateChat -> Chat with this name is already exists");
                return -1;
            }

            var members = new List<User>();

            members.Add(user);

            var chat = new Chat(chatCreatingDTO.Name, chatCreatingDTO.Icon, members, new List<Message>(), chatCreatingDTO.CreatorId, "", chatCreatingDTO.About);
            chat.SecondDialogMemberId = -1;

            try
            {
                var savedChat = await chatService.SaveChat(chat);
                return savedChat.Id;
            }
            catch (Exception e)
            {
                logger.LogError("CreateChat -> error: {0}", e.Message);
                return -1;
            }
        }

        // return -1 => error
        public async Task<long> CreateOrGetDialog(long creatorId, long secondUserId)
        {
            var creator = await userService.GetUser(creatorId);
            var secondUser = await userService.GetUser(secondUserId);

            if (creator == null || secondUser == null) return -1;

            string name = $"{creator.Username}{UserService.FORBIDDEN_SYMBOLS}{secondUser.Username}";
            string nameVariant = $"{secondUser.Username}{UserService.FORBIDDEN_SYMBOLS}{creator.Username}";

            var foundedChat1 = chatService.GetChatByNameExactly(name);
            var foundedChat2 = chatService.GetChatByNameExactly(nameVariant);

            if (foundedChat1 != null) return foundedChat1.Id;
            if (foundedChat2 != null) return foundedChat2.Id;

            var members = new List<User>();
            members.Add(creator);
            members.Add(secondUser);

            var chat = new Chat(name, creator.Icon, members, new List<Message>(), creatorId, "", null);
            chat.SecondDialogMemberId = secondUserId;

            try
            {
                Chat savedChat = await chatService.SaveChat(chat);
                return savedChat.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task<bool> UpdateChat(long id, ChatUpdatingDTO chatUpdatingDTO)
        {
            try
            {
                var foundedChat = await chatService.GetChat(id);
                if (foundedChat == null) return false;

                var nameTestChat = chatService.GetChatByNameExactly(chatUpdatingDTO.Name);
                if (nameTestChat.Id != chatUpdatingDTO.ChatId) return false;

                foundedChat.Name = chatUpdatingDTO.Name;
                foundedChat.About = chatUpdatingDTO.About;
                foundedChat.Icon = chatUpdatingDTO.Icon;
                foundedChat.CreatorId = chatUpdatingDTO.CreatorId;

                return await chatService.UpdateChat(id, foundedChat);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteChat(long chatId, long currentUserId)
        {
            var foundedChat = await chatService.GetChat(chatId);
            if (foundedChat.CreatorId != currentUserId && foundedChat.SecondDialogMemberId == -1) return false;
            else if (foundedChat.CreatorId != currentUserId && foundedChat.SecondDialogMemberId != currentUserId) return false;

            await chatService.DeleteChat(chatId);
            return true;
        }

        public List<ChatInfoDTO> FindChatsByName(string name)
        {
            if (name == null || name.Replace(" ", "") == "") return null;

            var foundedChats = chatService.FindChatsByName(name);
            if (foundedChats == null || foundedChats.Count == 0) return new List<ChatInfoDTO>();

            var chatsDTO = new List<ChatInfoDTO>();
            foreach (var chat in foundedChats)
            {
                chatsDTO.Add(new ChatInfoDTO(chat, userService));
            }

            return chatsDTO;
        }

        public async Task<bool> AddUserToChat(long chatId, long userId)
        {
            var foundedChat = await chatService.GetChat(chatId);
            var foundedUser = await userService.GetUser(userId);

            if (foundedChat == null || foundedUser == null) return false;

            if (!foundedChat.Members.Contains(foundedUser))
            {
                foundedChat.Members.Add(foundedUser);
                return await chatService.UpdateChat(chatId, foundedChat);
            }
            else return true;
        }

        public async Task<bool> DeleteUserFromChat(long chatId, long userId)
        {
            // Админа нельзя удалить, сначала его надо лишить полномочий админа

            var foundedChat = await chatService.GetChat(chatId);
            var foundedUser = await userService.GetUser(userId);

            if (foundedChat == null || foundedUser == null) return false;

            if (foundedChat.GetListOfAdminIds().Contains(userId)) return false;
            if (foundedChat.CreatorId == userId || foundedChat.SecondDialogMemberId == userId) return false; 

            foundedChat.Members.Remove(foundedUser);

            return await chatService.UpdateChat(chatId, foundedChat);
        }

        public async Task<List<long>> GetChatAdmins(long chatId)
        {
            var foundedChat = await chatService.GetChat(chatId);
            if (foundedChat == null) return null;

            return foundedChat.GetListOfAdminIds();
        }

        public async Task<bool> AddAdminToChat(long chatId, long adminId)
        {
            var foundedChat = await chatService.GetChat(chatId);
            var foundedUser = await userService.GetUser(adminId);

            if (foundedChat == null || foundedUser == null) return false;

            if (!foundedChat.Members.Contains(foundedUser)) return false;
            if (foundedChat.GetListOfAdminIds().Contains(adminId)) return false;
            if (foundedChat.CreatorId == adminId || foundedChat.SecondDialogMemberId == adminId) return false;

            var admins = foundedChat.GetListOfAdminIds();
            admins.Add(adminId);

            foundedChat.AdministratorIds = Chat.GetStringOfAdminIds(admins);

            return await chatService.UpdateChat(chatId, foundedChat);
        }

        public async Task<bool> DeleteAdmin(long chatId, long adminId)
        {
            var foundedChat = await chatService.GetChat(chatId);
            var foundedUser = await userService.GetUser(adminId);

            if (foundedChat == null || foundedUser == null) return false;

            var admins = foundedChat.GetListOfAdminIds();
            admins.Remove(adminId);

            foundedChat.AdministratorIds = Chat.GetStringOfAdminIds(admins);

            return await chatService.UpdateChat(chatId, foundedChat);
        }
    }
}
