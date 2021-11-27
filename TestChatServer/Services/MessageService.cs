using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestChatServer.Data.Entity;
using TestChatServer.Data.Repositories;

namespace TestChatServer.Services
{
    public class MessageService
    {
        private readonly IRepository<Message> _messageRepo;

        public MessageService(IRepository<Message> messageRepo)
        {
            _messageRepo = messageRepo;
        }

        public async Task<bool> SaveMessage(Message message)
        {
            try
            {
                await _messageRepo.AddAsync(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Message> GetMessage(long id)
        {
            return await _messageRepo.GetAsync(id);
        }

        public long GetTotalMessagesCount()
        {
            return _messageRepo.Items.LongCount();
        }

        public long GetChatMessagesCount(long chatId)
        {
            return _messageRepo.Items
                .Where(m => m.Chat.Id == chatId)
                .Count();
        }

        public List<Message> GetAllMessages()
        {
            return _messageRepo.Items
                .OrderBy(m => m.PubDate)
                .ToList();
        }

        public List<Message> GetAllMessagesByChat(long chatId)
        {
            return _messageRepo.Items
                .Where(m => m.Chat.Id == chatId)
                .OrderBy(m => m.PubDate)
                .ToList();
        }

        public List<Message> GetNMessagesByIndexInChat(long chatId, int messageCount, int startIndex)
        {
            var invertedMessages = _messageRepo.Items
                .Where(m => m.Chat.Id == chatId)
                .OrderByDescending(m => m.PubDate);

            return invertedMessages
                .Skip(startIndex)
                .Take(messageCount)
                .OrderBy(m => m.PubDate)
                .ToList();
        }

        public List<Message> GetMessagesByUser(long userId)
        {
            return _messageRepo.Items
                .Where(m => m.Author.Id == userId)
                .OrderBy(m => m.PubDate)
                .ToList();
        }

        public async Task DeleteMessage(long messageId)
        {
            await _messageRepo.RemoveAsync(messageId);
        }
    }
}
