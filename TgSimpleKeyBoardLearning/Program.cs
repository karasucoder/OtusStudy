using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace TgSimpleKeyBoardLearning
{
    internal class Program
    {
        private static string _token = "";

        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource(); // прерыватель соединения с ботом

            var bot = new TelegramBotClient("7852079891:AAEZYwrTU1uXquRBkx4lUfZUgqe8bEmSKSE", cancellationToken: cts.Token);

            // настройка параметров получения обновлений
            var receiverOptions = new ReceiverOptions
            {
                // только обновления типа message
                AllowedUpdates = [UpdateType.Message],
                // игнорирование обновлений, отправленных до запуска бота
                DropPendingUpdates = true
            };

            // создание обработчика обновлений
            var handler = new UpdateHandler();

            // получение обновлений
            bot.StartReceiving(handler, receiverOptions);
        }
    }
}
