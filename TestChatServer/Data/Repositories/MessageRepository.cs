using HabitAppServer.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestChatServer.Data.Entity;

namespace TestChatServer.Data.Repositories
{
    public class MessageRepository : DBRepository<Message>
    {
        public MessageRepository(ApplicationContext context) : base(context) { }

        public override IQueryable<Message> Items => base.Items.Include(item => item.Author).Include(item => item.Chat);
    }
}
