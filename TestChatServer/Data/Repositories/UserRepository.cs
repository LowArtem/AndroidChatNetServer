using HabitAppServer.Data.Repositories;
using TestChatServer.Data.Entity;

namespace TestChatServer.Data.Repositories
{
    public class UserRepository : DBRepository<User>
    {
        public UserRepository(ApplicationContext context) : base(context) { }
    }
}
