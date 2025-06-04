using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WEventRegistrationBotApp.Utilities;

namespace WEventRegistrationBotApp.Services;

public class MenuService
{
    private readonly ITelegramBotClient _botClient;

    public MenuService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ShowMenuAsync(long chatId, CancellationToken cancellationToken)
    {
        var menu = new InlineKeyboardMarkup(new[]
        {
            new[] { TelegramButtons.ShowEventScheduleButton },
            new[] { TelegramButtons.EventReservationButton },
            new[] { TelegramButtons.ManagerContactButton },
        });

        await _botClient.SendMessage(chatId: chatId, "Добро пожаловать в бот для записи на винные мероприятия! 🍷\n" +
                                                     "Здесь Вы можете узнать актуальное расписание мероприятий и забронировать место.\n\n" +
                                                     "Если у Вас есть вопросы, нажмите кнопку «Связаться с организатором».",
            parseMode: ParseMode.Html,
            replyMarkup: menu,
            cancellationToken: cancellationToken);
   }
}
