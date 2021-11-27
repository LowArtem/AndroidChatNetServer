using Microsoft.Extensions.DependencyInjection;

namespace TestChatServer.BL
{
    public static class BLRegistrator
    {
        public static IServiceCollection AddBLs(this IServiceCollection services) => services
            .AddScoped<ChatBL>()
            .AddScoped<MessageBL>()
            .AddScoped<UserBL>()
            ;
    }
}
