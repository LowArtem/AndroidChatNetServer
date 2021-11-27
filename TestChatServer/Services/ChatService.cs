using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;
using TestChatServer.Data.Repositories;

namespace TestChatServer.Services
{
    public class ChatService
    {
        private readonly IRepository<Chat> _chatRepo;
        private readonly IRepository<User> _userRepo;

        public ChatService(IRepository<Chat> chatRepo, IRepository<User> userRepo)
        {
            _chatRepo = chatRepo;
            _userRepo = userRepo;
        }

        public List<Chat> GetAllChats()
        {
            try
            {
                var chats = _chatRepo.Items
                    .Where(c => c.Messages != null && c.Messages.Count > 0)
                    .OrderByDescending(c => c.Messages.LastOrDefault().PubDate);

                var nullChats = _chatRepo.Items
                    .Where(c => c.Messages == null || c.Messages.Count == 0);

                if ((nullChats == null || nullChats.Count() == 0) && (chats != null && chats.Count() > 0))
                    return chats.ToList();
                else if (chats == null || chats.Count() == 0)
                    return new List<Chat>();
                
                return nullChats.Concat(chats).ToList();
            }
            catch (Exception)
            {
                return _chatRepo.Items.ToList();
            }
        }

        public async Task<Chat> GetChat(long chatId)
        {
            return await _chatRepo.GetAsync(chatId);
        }

        public async Task<List<Chat>> GetAllChatsByUser(long userId)
        {
            User user = await _userRepo.GetAsync(userId);

            return GetAllChats().Where(c => c.Members.Contains(user)).ToList();
        }

        public async Task<Chat> SaveChat(Chat chat)
        {
            try
            {
                return await _chatRepo.AddAsync(chat);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateChat(long id, Chat chat)
        {
            try
            {
                var oldChat = await _chatRepo.GetAsync(id);
                if (oldChat == null) return false;

                oldChat.Name = chat.Name;
                oldChat.About = chat.About;
                oldChat.Icon = chat.Icon;

                oldChat.Members = chat.Members;
                oldChat.Messages = chat.Messages;
                oldChat.CreatorId = chat.CreatorId;
                oldChat.AdministratorIds = chat.AdministratorIds;

                await _chatRepo.UpdateAsync(oldChat);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task DeleteChat(long id)
        {
            try
            {
                await _chatRepo.RemoveAsync(id);
            }
            catch (Exception)
            {
            }
        }

        public List<Chat> FindChatsByName(string name)
        {
            return _chatRepo.Items.Where(c => c.Name.Replace(" ", "").ToLower().Contains(name.Replace(" ", "").ToLower())).ToList();
        }

        public Chat GetChatByNameExactly(string name)
        {
            return _chatRepo.Items.Where(c => c.Name.ToLower() == name.ToLower()).SingleOrDefault();
        }
    }
}
