using HabitAppServer.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TestChatServer.Data.Entity;

namespace TestChatServer.Data.Repositories
{
    public class ChatRepository : DBRepository<Chat>
    {
        public ChatRepository(ApplicationContext context) : base(context) { }

        public override IQueryable<Chat> Items => base.Items.Include(item => item.Members).Include(item => item.Messages);
    }
}
