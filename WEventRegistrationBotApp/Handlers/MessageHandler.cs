using Telegram.Bot.Types;
using Telegram.Bot;
using WEventRegistrationBotApp.Services;
using WEventRegistrationBotApp.Data.Models.State;

namespace WEventRegistrationBotApp.Handlers;

public class MessageHandler
{
    private readonly GuestStateManager _stateManager;
    private readonly ITelegramBotClient _botClient;
    private readonly MenuService _menuService;
    private readonly EventReservationService _reservationService;

    public MessageHandler(
        GuestStateManager stateManager, 
        ITelegramBotClient botClient,
        MenuService menuService,
        EventReservationService eventReservationService)
    {
        _stateManager = stateManager;
        _botClient = botClient;
        _menuService = menuService;
        _reservationService = eventReservationService;
    }

    public async Task HandleMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var state = _stateManager.GetOrCreateGuestState(message.Chat.Id);

        if (state.CurrentStep != EventReservationStep.None)
        {
            await HandleReservationStepAsync(state, message, cancellationToken);
            return;
        }

        switch (message.Text)
        {
            case "/start":
                await _menuService.ShowMenuAsync(message.Chat.Id, cancellationToken);
                break;
            default:
                await _botClient.SendMessage(
                    chatId: message.Chat.Id, 
                    text: "Я не знаю такой команды. Попробуйте ещё раз.",
                    cancellationToken: cancellationToken);
                break;
        }
    }

    private async Task HandleReservationStepAsync(GuestState state, Message message, CancellationToken cancellationToken)
    {
        switch (state.CurrentStep)
        {
            case EventReservationStep.EnteringGuestCount when int.TryParse(message.Text, out var guestCount):
                await _reservationService.ValidateGuestsCountAsync(state.ChatId, guestCount, cancellationToken);
                break;

            case EventReservationStep.EnteringGuestPhoneNumber:
                if (message.Contact != null)
                {
                    await _reservationService.HandlePhoneNumberInputAsync(
                        state.ChatId,
                        message.Contact.PhoneNumber,
                        cancellationToken);
                }
                else
                {
                    await _reservationService.HandlePhoneNumberInputAsync(
                        state.ChatId,
                        message.Text!,
                        cancellationToken);
                }
                break;

            case EventReservationStep.EnteringGuestName:
                await _reservationService.HandleGuestNameInputAsync(
                    state.ChatId,
                    message.Text!,
                    cancellationToken);
                break;

            default:
                await _botClient.SendMessage(
                    chatId: state.ChatId,
                    text: "Пожалуйста, следуйте инструкциям для завершения бронирования.",
                    cancellationToken: cancellationToken);
                break;
        }
    }
}
