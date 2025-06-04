using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WEventRegistrationBotApp.Data;
using WEventRegistrationBotApp.Data.Models;
using WEventRegistrationBotApp.Data.Models.State;
using WEventRegistrationBotApp.Utilities;

namespace WEventRegistrationBotApp.Services
{
    public class EventReservationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ApplicationContext _dbContext;
        private readonly GuestStateManager _guestStateManager;
        private readonly ReservationCostCalculationService _costCalculationService;
        private readonly MenuService _menuService;

        public EventReservationService(
            ITelegramBotClient botClient,
            ApplicationContext dbContext,
            GuestStateManager guestStateManager,
            ReservationCostCalculationService costCalculationService,
            MenuService menuService)
        {
            _botClient = botClient;
            _dbContext = dbContext;
            _guestStateManager = guestStateManager;
            _costCalculationService = costCalculationService;
            _menuService = menuService;
        }

        public async Task ShowAvailableEventsAsync(long chatId, CancellationToken cancellationToken)
        {
            var state = _guestStateManager.GetOrCreateGuestState(chatId);

            state.CurrentStep = EventReservationStep.SelectingEvent;

            var availableEvents = await _dbContext.WineEvents
                .Where(x => x.EventDate > DateTime.UtcNow.Date.AddDays(1)) // запрет бронирования мероприятий за сутки
                .Select(x => new
                {
                    Event = x,
                    FreeSeats = x.MaxParticipants - x.Reservations.Where(s => s.Status != ReservationStatus.Canceled).Sum(s => s.GuestCount)
                })
                .Where(x => x.FreeSeats > 0)
                .OrderBy(x => x.Event.EventDate)
                .ToArrayAsync(cancellationToken);

            if (availableEvents.Length == 0)
            {
                await _botClient.SendMessage(
                    chatId: chatId,
                    text: "В этом месяце не осталось активных мероприятий. Следите за анонсами!",
                    cancellationToken: cancellationToken);

                _guestStateManager.ResetGuestState(chatId);

                return;
            }

            var buttons = availableEvents.Select(e =>
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: $"{e.Event.Name} | {e.Event.EventDate:dd.MM.} | {e.Event.Price} | 🆓 {e.FreeSeats} мест",
                        callbackData: $"select_event_{e.Event.Id}")
                }
            ).ToArray();

            await _botClient.SendMessage(
                chatId: chatId,
                text: "Выберите мероприятие:",
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }

        public async Task HandleEventSelectionAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var state = _guestStateManager.GetOrCreateGuestState(callbackQuery.Message!.Chat.Id);

            state.CurrentStep = EventReservationStep.EnteringGuestCount;

            var selectedEventId = int.Parse(callbackQuery.Data!.Substring(13));

            state.SelectedEventId = selectedEventId;

            var buttons = new[]
            {
                new[] { InlineKeyboardButton.WithCallbackData("1", "guests_1") },
                new[] { InlineKeyboardButton.WithCallbackData("2", "guests_2") },
                new[] { InlineKeyboardButton.WithCallbackData("3", "guests_3") },
                new[] { InlineKeyboardButton.WithCallbackData("Другое количество", "guests_custom") }
            };

            await _botClient.SendMessage(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Укажите количество гостей:",
                replyMarkup: new InlineKeyboardMarkup(buttons),
                cancellationToken: cancellationToken);
        }

        public async Task HandleGuestsCountSelectionAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery.Data != "guests_custom")
            {
                //var guestCount = int.Parse(callbackQuery.Data!.Replace("guests_", ""));
                var guestCount = int.Parse(callbackQuery.Data!.Substring(7));

                await ValidateGuestsCountAsync(callbackQuery.Message!.Chat.Id, guestCount, cancellationToken);
            }
            else
            {
                await _botClient.SendMessage(
                    chatId: callbackQuery.Message!.Chat.Id,
                    text: "Введите количество гостей:",
                    cancellationToken: cancellationToken);

                return;
            }
        }

        public async Task ValidateGuestsCountAsync(long chatId, int guestCount, CancellationToken cancellationToken)
        {
            var state = _guestStateManager.GetOrCreateGuestState(chatId);

            state.GuestCount = guestCount;

            var freeSeats = await GetFreeSeats(state.SelectedEventId);

            if (guestCount > freeSeats)
            {
                await _botClient.SendMessage(
                    chatId: chatId,
                    text: $"Количество гостей превышает доступное для бронирования количество мест - {freeSeats}.\n" +
                          $"Укажите меньшее число гостей или выберите другое мероприятие.",
                    replyMarkup: TelegramButtons.ChangeEventForReservationButton,
                    cancellationToken: cancellationToken);
                return;
            }

            await CheckGuestExistsAsync(chatId, cancellationToken);
        }

        private async Task<int> GetFreeSeats(int eventId)
        {
            var wineEvent = await _dbContext.WineEvents
                .Include(e => e.Reservations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (wineEvent == null)
                return 0;

            var bookedSeats = wineEvent.Reservations
                .Count(r => r.Status != ReservationStatus.Canceled);

            return wineEvent.MaxParticipants - bookedSeats;
        }

        // проверяет, есть ли гость в БД, чтобы не запрашивать каждый раз контакты
        private async Task CheckGuestExistsAsync(long chatId, CancellationToken cancellationToken)
        {
            var state = _guestStateManager.GetOrCreateGuestState(chatId);

            var guest = await _dbContext.Guests
                .FirstOrDefaultAsync(g => g.TelegramId == chatId, cancellationToken);

            if (guest != null)
            {
                state.GuestName = guest.Name;
                state.GuestPhoneNumber = guest.PhoneNumber;

                await HandleGuestNameInputAsync(chatId, guest.Name, cancellationToken);
            }
            else
            {
                state.CurrentStep = EventReservationStep.EnteringGuestPhoneNumber;

                await _botClient.SendMessage(
                    chatId: chatId,
                    text: "Укажите номер телефона для связи. Введите его вручную или нажмите клавишу «Поделиться контактом».",
                    replyMarkup: new ReplyKeyboardMarkup(
                        new[] { new KeyboardButton("Поделиться контактом") { RequestContact = true } })
                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true
                    },
                    cancellationToken: cancellationToken);
            }
        }

        public async Task HandlePhoneNumberInputAsync(long chatId, string phoneNumber, CancellationToken cancellationToken)
        {
            var state = _guestStateManager.GetOrCreateGuestState(chatId);

            state.GuestPhoneNumber = phoneNumber;

            state.CurrentStep = EventReservationStep.EnteringGuestName;

            await _botClient.SendMessage(
                replyMarkup: new ReplyKeyboardRemove(),
                chatId: chatId,
                text: "Введите Ваше имя.",
                cancellationToken: cancellationToken);
        }

        public async Task HandleGuestNameInputAsync(long chatId, string guestName, CancellationToken cancellationToken)
        {
            var state = _guestStateManager.GetOrCreateGuestState(chatId);

            state.GuestName = guestName;

            state.CurrentStep = EventReservationStep.ConfirmingReservation;

            var wineEvent = await _dbContext.WineEvents
                .FirstOrDefaultAsync(e => e.Id == state.SelectedEventId, cancellationToken);

            var reservationCost = await _costCalculationService.CalculateReservationCostAsync(chatId, state.GuestCount, wineEvent!.Price);

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { TelegramButtons.ConfrimReservationButton },
                new[] { TelegramButtons.CancelReservationButton }
            });

            await _botClient.SendMessage(
                chatId: chatId,
                text: $"Проверьте данные бронирования:\n\n" +
                      $"Мероприятие: {wineEvent.Name}\n" +
                      $"Дата и время: {wineEvent.EventDate:dd.MM} {wineEvent.EventDate:t}\n" +
                      $"Место проведения: {wineEvent.Location}\n" +
                      $"Количество гостей: {state.GuestCount}\n" +
                      $"Имя: {state.GuestName}\n" +
                      $"Телефон: {state.GuestPhoneNumber}\n" +
                      $"К оплате: {reservationCost.ToString("F2")}\n\n" +
                      $"Подтверждаете бронирование?",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken);
        }

        public async Task HandleReservationConfirmationAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            var state = _guestStateManager.GetOrCreateGuestState(callbackQuery.Message!.Chat.Id);

            var freeSeats = await GetFreeSeats(state.GuestCount);

            if (state.GuestCount > freeSeats)
            {
                await _botClient.SendMessage(
                    chatId: callbackQuery.Message!.Chat.Id,
                    text: "Извините, но кто-то уже успел забронировать место на данное мероприятие.\n" +
                          $"На данный момент осталось свободно {freeSeats}.\n" +
                          "Текущая запись отменена. Пожалуйста, повторите запись или выберите другое мероприятие.",
                    cancellationToken: cancellationToken);

                _guestStateManager.ResetGuestState(callbackQuery.Message!.Chat.Id);
            }

            var guest = await _dbContext.Guests
                .FirstOrDefaultAsync(g => g.TelegramId == callbackQuery.Message.Chat.Id, cancellationToken);
            
            if (guest == null)
            {
                guest = new Guest
                {
                    TelegramId = callbackQuery.Message.Chat.Id,
                    Name = state.GuestName!,
                    PhoneNumber = state.GuestPhoneNumber!,
                    RegistrationDate = DateTime.UtcNow
                };
            }

            var reservation = new Reservation
            {
                Guest = guest,
                WineEventId = state.SelectedEventId,
                MainGuestId = guest.GuestId,
                GuestCount = state.GuestCount,
                Source = ReservationSource.Telegram,
                Status = ReservationStatus.Pending,
            }; 
            
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            var wineEvent = await _dbContext.WineEvents
                .FirstOrDefaultAsync(e => e.Id == state.SelectedEventId, cancellationToken);
            
            await _botClient.SendMessage(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Ваша заявка принята! Менеджер свяжется с вами для подтверждения.",
                cancellationToken: cancellationToken);
            
            await _botClient.SendMessage(
                chatId: $"{AppConfiguration.GroupChatId}",
                text: "Есть новая бронь!\n\n" +
                $"Мероприятие: {wineEvent!.Name}\n" +
                $"Количество гостей: {state.GuestCount}\n" +
                $"Имя главного гостя: {state.GuestName}\n" +
                $"Telegram: https://t.me/{state.ChatId}\n" +
                $"Телефон для связи: {state.GuestPhoneNumber}");
            
            _guestStateManager.ResetGuestState(callbackQuery.Message.Chat.Id);
        }

        public async Task HandleReservationCancellationAsync(CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            if (callbackQuery.Data != "cancel_reservation")
            {
                return;
            }

            _guestStateManager.ResetGuestState(callbackQuery.Message!.Chat.Id);

            await _botClient.SendMessage(
                chatId: callbackQuery.Message.Chat.Id,
                text: "Заявка отменена.",
                cancellationToken: cancellationToken);

            await _menuService.ShowMenuAsync(callbackQuery.Message!.Chat.Id, cancellationToken);
        }
    }
}
