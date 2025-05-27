using Telegram.Bot;

namespace WEventRegistrationBotApp.Handlers;

public static class ErrorHandler
{
    public static Task HandlePollingErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Произошла ошибка: {exception.Message}");
        return Task.CompletedTask;
    }
}
