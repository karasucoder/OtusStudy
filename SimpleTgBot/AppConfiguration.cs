using Microsoft.Extensions.Configuration;

namespace SimpleOtusTelegramBotApp;

internal static class AppConfiguration
{
    public static readonly string BotToken;
    public static readonly string GroupChatId;
    public static readonly string EventChannelId;
    public static readonly string ManagerChatId;
    public static readonly string DefaultConnection;

    static AppConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        BotToken = configuration["BotToken"];
    }
}
