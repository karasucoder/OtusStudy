using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SimpleTgBot
{
    public delegate void MessageHandler(string message);

    public class UpdateHandler : IUpdateHandler
    {
        public event MessageHandler? OnHandleUpdateStarted;
        public event MessageHandler? OnHandleUpdateCompleted;

        public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Произошла ошибка: {exception.Message}");

            await Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update == null || update.Type != UpdateType.Message || update.Message?.Type != MessageType.Text)
                return;

            var messageText = update.Message.Text;

            try
            {
                OnHandleUpdateStarted?.Invoke(messageText);

                await botClient.SendMessage(
                    chatId: update.Message.Chat.Id,
                    text: "Сообщение успешно принято.",
                    cancellationToken: cancellationToken);
            }
            finally
            {
                OnHandleUpdateCompleted?.Invoke(messageText);
            }
        }
    }
}
