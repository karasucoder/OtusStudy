using Telegram.Bot;
using Telegram.Bot.Types;
using WEventRegistrationBotApp.Data.Models.State;
using WEventRegistrationBotApp.Services;

namespace WEventRegistrationBotApp.Handlers;

public class CallbackQueryHandler
{
    private readonly MenuService _menuService;
    private readonly EventScheduleService _scheduleService;
    private readonly EventReservationService _reservationService;
    private readonly ITelegramBotClient _botClient;
    private readonly GuestStateManager _guestStateManager;

    public CallbackQueryHandler(
        MenuService menuService,
        EventScheduleService scheduleService,
        EventReservationService eventReservationService,
        ITelegramBotClient botClient,
        GuestStateManager guestStateManager)
    {
        _menuService = menuService;
        _scheduleService = scheduleService;
        _reservationService = eventReservationService;
        _botClient = botClient;
        _guestStateManager = guestStateManager;
    }

    public async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        try
        {
            await _botClient.AnswerCallbackQuery(
                callbackQuery.Id,
                cancellationToken: cancellationToken);

            if (callbackQuery.Data!.StartsWith("select_event_"))
            {
                await _reservationService.HandleEventSelectionAsync(callbackQuery, cancellationToken);
                return;
            }

            else if (callbackQuery.Data.StartsWith("guests_"))
            {
                await _reservationService.HandleGuestsCountSelectionAsync(callbackQuery, cancellationToken);
                return;
            }

            switch (callbackQuery.Data)
            {
                case "show_event_schedule":
                    await _scheduleService.ShowEventScheduleAsync(callbackQuery.Message!.Chat.Id, cancellationToken);
                    break;
                case "event_reservation":
                    await _reservationService.ShowAvailableEventsAsync(callbackQuery.Message!.Chat.Id, cancellationToken);
                    break;
                case "change_event_for_reservation":
                    _guestStateManager.ResetGuestState(callbackQuery.Message!.Chat.Id);
                    await _reservationService.ShowAvailableEventsAsync(callbackQuery.Message!.Chat.Id, cancellationToken);
                    break;
                case "confirm_reservation":
                    await _reservationService.HandleReservationConfirmationAsync(callbackQuery, cancellationToken);
                    break;
                case "cancel_reservation":
                    await _reservationService.HandleReservationCancellationAsync(callbackQuery, cancellationToken);
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling callback: {ex.Message}");
            await _botClient.SendMessage(
                callbackQuery.Message!.Chat.Id,
                "Произошла ошибка при обработке запроса. Пожалуйста, попробуйте ещё раз.",
                cancellationToken: cancellationToken);
        }
    }
}
