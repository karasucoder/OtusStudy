using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SimpleTgBot
{
    public delegate void MessageHandler(string message);

    public class UpdateHandler : IUpdateHandler
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public event MessageHandler? OnHandleUpdateStarted;
        public event MessageHandler? OnHandleUpdateCompleted;

        public async Task HandleErrorAsync(
            ITelegramBotClient botClient, 
            Exception exception, 
            HandleErrorSource source, 
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {exception.Message}");
            await Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(
            ITelegramBotClient botClient, 
            Update update, 
            CancellationToken cancellationToken)
        {
            if (update == null || update.Type != UpdateType.Message || update.Message?.Type != MessageType.Text)
                return;

            var messageText = update.Message.Text;

            try
            {
                OnHandleUpdateStarted?.Invoke(messageText);

                switch (messageText)
                {
                    case "/start":
                        await botClient.SendMessage(
                            chatId: update.Message.Chat.Id,
                            text: "Добро пожаловать в бот! Используйте команду /cat для получения случайного факта о кошках.",
                            cancellationToken: cancellationToken);
                        break;
                    case "/cat":
                        await CatsRandomFactHandle(botClient, update.Message.Chat.Id, cancellationToken);
                        break;
                    default:
                        await botClient.SendMessage(
                            chatId: update.Message.Chat.Id,
                            text: "Сообщение успешно принято.",
                            cancellationToken: cancellationToken);
                        break;
                }
            }
            finally
            {
                OnHandleUpdateCompleted?.Invoke(messageText);
            }
        }

        private async Task CatsRandomFactHandle(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetStringAsync("https://catfact.ninja/fact", cancellationToken);
                var jsonDoc = JsonDocument.Parse(response);
                var fact = jsonDoc.RootElement.GetProperty("fact").GetString();

                await botClient.SendMessage(
                    chatId: chatId,
                    text: $"Случайный факт о кошках: {fact}",
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                await botClient.SendMessage(
                    chatId: chatId,
                    text: "Не удалось получить факт о кошках. Попробуйте повторить команду позже.",
                    cancellationToken: cancellationToken);

                Console.WriteLine($"Произошла ошибка при обработке запроса: {ex.Message}");
            }
        }
    }
}
