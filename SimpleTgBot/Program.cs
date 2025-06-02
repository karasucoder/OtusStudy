using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace SimpleTgBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            try
            {
                var botClient = new TelegramBotClient(AppConfiguration.BotToken);
                var handler = new UpdateHandler();

                handler.OnHandleUpdateStarted += message =>
                    Console.WriteLine($"Началась обработка сообщения '{message}'");

                handler.OnHandleUpdateCompleted += message =>
                    Console.WriteLine($"Закончилась обработка сообщения '{message}'");

                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = [UpdateType.Message],
                    DropPendingUpdates = true
                };

                botClient.StartReceiving(
                   updateHandler: handler,
                   receiverOptions: receiverOptions,
                   cancellationToken: cts.Token);

                var me = await botClient.GetMe(cancellationToken: cts.Token);

                Console.WriteLine($"{me.FirstName} запущен!");

                Console.WriteLine("\nНажмите клавишу A для выхода.");

                while (!cts.IsCancellationRequested)
                {
                    var key = Console.ReadKey();

                    if (key.Key == ConsoleKey.A)
                    {
                        Console.WriteLine("\nЗавершение работы...");

                        cts.Cancel();
                    }
                    else
                    {
                        var botInfo = await botClient.GetMe(cancellationToken: cts.Token);

                        Console.WriteLine($"\nBot ID: {botInfo.Id}\n" +
                                          $"Bot Username: {botInfo.Username}\n" +
                                          $"Bot Username: {botInfo.FirstName}\n");
                    }
                }
            }
            finally
            {
                cts.Dispose();
            }
        }
    }
}
