using Telegram.Bot;
using Telegram.Bot.Types;

namespace WEventRegistrationBotApp.Handlers;

public class UpdateHandler
{
    private readonly MessageHandler _messageHandler;
    private readonly CallbackQueryHandler _callbackQueryHandler;

    public UpdateHandler(MessageHandler messageHandler, CallbackQueryHandler callbackQueryHandler)
    {
        _messageHandler = messageHandler;
        _callbackQueryHandler = callbackQueryHandler;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update is null) return;

        if (update.Message is { } message && (message.Text is not null || message.Contact is not null))
        {
            await _messageHandler.HandleMessageAsync(message, cancellationToken);
        }

        if (update.CallbackQuery is { } callbackQuery)
        {
            await _callbackQueryHandler.HandleCallbackQueryAsync(botClient, callbackQuery, cancellationToken);
        }
    }
}
