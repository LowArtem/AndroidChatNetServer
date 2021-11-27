using Microsoft.Extensions.DependencyInjection;

namespace TestChatServer.Services
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddScoped<ChatService>()
            .AddScoped<MessageService>()
            .AddScoped<UserService>()
            ;
    }
}
