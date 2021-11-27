using Microsoft.Extensions.DependencyInjection;
using TestChatServer.Data.Entity;

namespace TestChatServer.Data.Repositories
{
    public static class RepositoryRegistrator
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services) => services
            .AddScoped<IRepository<User>, UserRepository>()
            .AddScoped<IRepository<Message>, MessageRepository>()
            .AddScoped<IRepository<Chat>, ChatRepository>()
            ;
    }
}
