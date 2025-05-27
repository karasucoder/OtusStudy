using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using WEventRegistrationBotApp.Data;
using WEventRegistrationBotApp.Data.Models.State;
using WEventRegistrationBotApp.Handlers;
using WEventRegistrationBotApp.Services;

namespace WEventRegistrationBotApp;

internal class BotApp
{
    private static TelegramBotClient _botClient;
    private static ApplicationContext _dbContext;
    private static GuestStateManager _guestStateManager;
    private static EventReservationService _reservationService;
    private static MenuService _menuService;
    private static EventScheduleService _scheduleService;
    private static ReservationCostCalculationService _costCalculationService;
    private static MessageHandler _messageHandler;
    private static CallbackQueryHandler _callbackQueryHandler;
    private static UpdateHandler _updateHandler;

    public BotApp()
    {
        _botClient = new TelegramBotClient(AppConfiguration.BotToken);

        _dbContext = new ApplicationContext();

        _guestStateManager = new GuestStateManager();

        _costCalculationService = new ReservationCostCalculationService(_botClient, _dbContext);

        _menuService = new MenuService(_botClient);

        _scheduleService = new EventScheduleService(_botClient, _dbContext);

        _reservationService = new EventReservationService(
            _botClient,
            _dbContext,
            _guestStateManager,
            _costCalculationService,
            _menuService);

        _messageHandler = new MessageHandler(
            _guestStateManager,
            _botClient,
            _menuService,
            _reservationService);

        _callbackQueryHandler = new CallbackQueryHandler(
            _menuService,
            _scheduleService,
            _reservationService,
            _botClient,
            _guestStateManager);

        _updateHandler = new UpdateHandler(_messageHandler, _callbackQueryHandler);
    }

    public async Task Start()
    {
        using var cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery]
        };

        _botClient.StartReceiving(
            updateHandler: _updateHandler.HandleUpdateAsync,
            errorHandler: (s, x, h, _) =>
            {
                Console.WriteLine($"Произошла ошибка: {x.Message}");
                return Task.CompletedTask; 
            },
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token);

        var me = await _botClient.GetMe();

        Console.WriteLine($"Бот {me.Username} запущен!");

        await Task.Delay(-1, cts.Token).ConfigureAwait(false);
    }
}
