using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WineEventRegistrationBotApp;

public class Program
{
    private static string _token = "8188745354:AAEQtp93f7z6zlnlM1cHOXuhhFo4Bfg_jkM";

    private static TelegramBotClient? _botClient;

    static async Task Main(string[] args)
    {
        using var cts = new CancellationTokenSource();

        _botClient = new TelegramBotClient(_token, cancellationToken: cts.Token);

        var me = await _botClient.GetMe();

        _botClient.OnError += OnError;

        _botClient.OnMessage += OnMessage;

        _botClient.OnUpdate += OnUpdate;

        Console.WriteLine($"Бот {me.Username} запущен!");

        Console.ReadLine();

        cts.Cancel();
    }

    // метод обработки ошибок
    static async Task OnError(Exception exception, HandleErrorSource errorSource)
    {
        Console.WriteLine($"Error: {exception.Message}");
    }

    // метод обработки сообщений от бота
    static async Task OnMessage(Message msg, UpdateType type)
    {
        if (msg.Text == "/start")
        {
            await ShowMenu(msg);
        }

        Console.WriteLine($"Получен {type} '{msg.Text}' in {msg.Chat}");

        await _botClient.SendMessage(msg.Chat, $"{msg.From} said: {msg.Text}");
    }

    // метод обработки других типов обновлений от бота
    static async Task OnUpdate(Telegram.Bot.Types.Update update)
    {
        if (update is { CallbackQuery: { } query })
        {
            switch (query.Data)
            {
                case "Назад":
                    await ShowMenu(query.Message!);
                    break;
                case "Расписание мероприятий":
                    var wineEventsList = await ShowWineEventsSchedule();
                    var messageText = " ";
                    foreach (var wineEvent in wineEventsList)
                    {
                        messageText += $"📅 <b>Дата:</b> {wineEvent.Date:dd.MM}\n";
                        messageText += $"🏷 <b>Название:</b> {wineEvent.Name}\n";
                        messageText += $"📝 <b>Описание:</b> {wineEvent.Description}\n";
                        messageText += $"📍 <b>Место:</b> {wineEvent.Location}\n\n";
                    }
                    await _botClient.SendMessage(query.Message!.Chat, messageText,
                        ParseMode.Html,
                        replyMarkup: new InlineKeyboardButton[] { "Назад", "Записаться на мероприятие" });
                    break;
                case "Записаться на мероприятие":
                    // получение списка названий мероприятий
                    using (ApplicationContext db = new ApplicationContext())
                    {
                        var wineEventList = await db.WineEvents
                            .Select(e => e.Name)
                            .ToListAsync();

                        // создание кнопок с названиями мероприятий
                        var buttons = wineEventList
                            .Select(wineEventName => new KeyboardButton(wineEventName))
                            .ToArray();

                        var replyKeyboard = new ReplyKeyboardMarkup(buttons)
                        {
                            ResizeKeyboard = true
                        };

                        await _botClient.SendMessage(query.Message!.Chat, "На какое мероприятие Вы хотели бы записаться?",
                            replyMarkup: replyKeyboard);
                    break;
                    }
            }
        }
    }

    // метод отображения главного меню
    static async Task ShowMenu(Message msg)
    {
        await _botClient.SendMessage(msg.Chat, "Добро пожаловать в бот для записи на винные мероприятия!",
                ParseMode.Html,
                replyMarkup: new InlineKeyboardButton[] { ("Расписание мероприятий"), ("Связаться с организатором", "https://t.me/+79897089675") });
    } 

    // метод получения списка мероприятий
    static async Task<List<WineEvent>> ShowWineEventsSchedule()
    {
        using (ApplicationContext db = new ApplicationContext())
        {
           return await db.WineEvents
                .OrderBy(e => e.Date)
                .ToListAsync();
        }
    }

    // метод создания брони
    static async Task CreateReservation()
    {
    }

    // метод расчета скидки для брони
    static async Task<int> CalculateDiscount(int userId, int phoneNumber, int guestAmount)
    {
        var userTelegramId = userId;

        var discount = 0;

        // если гостей меньше 3
        if (guestAmount < 3)
        {
            // проверить кол-во посещений
            using (ApplicationContext db = new ApplicationContext())
            {
                // поиск гостя по номеру телефона
                var guest = await db.Guests
                    .FirstOrDefaultAsync(g => g.PhoneNumber.Equals(phoneNumber));

                // если гость найден
                if (guest != null)
                {
                    var guestVisitsNumber = await db.Reservations
                        .Where(r => r.MainGuestId == guest.Id)
                        .CountAsync();

                    Console.WriteLine($"Гость найден. Количество посещений: {guestVisitsNumber}");

                    // если кол-во посещений >=1, то начислить скидку 10%
                    if (guestVisitsNumber >= 1)
                    {
                        discount = 10;

                        return discount;
                    }
                    else
                    {
                        // проверить, является ли подписчиком канала
                        if (!await IsUserSubscribedAsync(userTelegramId))
                        {
                            return discount;
                        }

                        discount = 10;

                        return discount;
                    }
                }
                else
                {
                    // проверить, является ли подписчиком канала
                    if (!await IsUserSubscribedAsync(userTelegramId))
                    {
                        return discount;
                    }

                    discount = 10;

                    return discount;
                }
            }
        }
        // если кол-во гостей больше или равно 3
        // каждому начислить скидку 10%
        else if (guestAmount >= 3)
        {
            discount = 10 * guestAmount;

            return discount;
        }
        else
        {
            // скидка 0%
            return discount;
        }
    }

    // метод проверки, является ли гость подписчиком канала
    static async Task<bool> IsUserSubscribedAsync(int userId)
    {
        var userTelegramId = userId;

        ChatMember chatMember = await _botClient.GetChatMember("barathome", userTelegramId);

        if (chatMember.Status is ChatMemberStatus.Member)
        {
            return true;
        }

        return false;
    }
    }

