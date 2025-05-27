using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WEventRegistrationBotApp.Data;
using WEventRegistrationBotApp.Data.Models;
using WEventRegistrationBotApp.Utilities;

namespace WEventRegistrationBotApp.Services;

public class EventScheduleService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ApplicationContext _dbContext;

    public EventScheduleService(ITelegramBotClient botClient, ApplicationContext dbContext)
    {
        _botClient = botClient;
        _dbContext = dbContext;
    }

    public async Task ShowEventScheduleAsync(long chatId, CancellationToken cancellationToken)
    {
        string message;

        InlineKeyboardMarkup inlineKeyboard;

        try
        {
            var events = await GetCurrentEventsAsync();

            if (events.Length > 0)
            {
                message = $"<b>Афиша мероприятий на месяц:</b>\n\n" +
                          string.Join("\n\n", events.Select(e =>
                              $"<b>{e.Name}</b>\n" +
                              $"{e.Description}\n\n" +
                              $"📅 {e.EventDate:dd.MM}\n" +
                              $"📍 {e.Location}\n" +
                              $"💰 Цена: {e.Price} руб."));

                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                        new[] { TelegramButtons.EventReservationButton },
                        new[] { TelegramButtons.ManagerContactButton }
                });
            }
            else
            {
                message = "На этот месяц мероприятий не запланировано.\n" +
                "Хотите предложить своё мероприятие или уточнить детали?";

                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new[] { TelegramButtons.ManagerContactButton }
                });
            }
        }
        catch (Exception e)
        {
            message = "Кто-то налил этому боту слишком много вина, и он решил ненадолго прилечь. 🥴\n\n" +
                "Наши сомелье уже бегут на помощь!\n" +
                "Для записи на мероприятие свяжитесь с организатором.";

            Console.WriteLine(e.Message);

            inlineKeyboard = new InlineKeyboardMarkup(new[] { TelegramButtons.ManagerContactButton });
        }

        await _botClient.SendMessage(
            chatId: chatId,
            text: message,
            parseMode: ParseMode.Html,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task<WineEvent[]> GetCurrentEventsAsync()
    {
        var currentDate = DateTime.UtcNow.Date;
        var firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

        return await _dbContext.WineEvents
            .Where(x => x.EventDate >= firstDayOfMonth &&
                        x.EventDate <= lastDayOfMonth)
            .OrderBy(x => x.EventDate)
            .ToArrayAsync();
    }
}
