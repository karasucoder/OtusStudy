using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace SimpleTgBot
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            var me = await bot.GetMe();

            Console.WriteLine($"{me.FirstName} запущен!");

            // бесконечная задержка
            // чтобы программа не завершалась сразу после запуска бота
            await Task.Delay(-1);

            await cts.CancelAsync(); // отмена операций перед выходом из программы
        }
    }
}
