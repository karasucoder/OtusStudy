namespace WEventRegistrationBotApp;

public class Program
{
    static async Task Main(string[] args)
    {
        BotApp app = new BotApp();
        await app.Start();
    }
}
