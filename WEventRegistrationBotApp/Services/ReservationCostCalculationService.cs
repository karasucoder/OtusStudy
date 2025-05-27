using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WEventRegistrationBotApp.Data;
using WEventRegistrationBotApp.Data.Models;

namespace WEventRegistrationBotApp.Services;

public class ReservationCostCalculationService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ApplicationContext _dbContext;

    public ReservationCostCalculationService(ITelegramBotClient botClient, ApplicationContext dbContext)
    {
        _botClient = botClient;
        _dbContext = dbContext;
    }

    public async Task<decimal> CalculateReservationCostAsync(long chatId, int guestCount, decimal wineEventPrice)
    {
        decimal discountPercent;

        if (guestCount >= 3 || await IsRegularGuest(chatId))
        {
            discountPercent = 10m;
        }
        else if(await IsUserSubscribedAsync(chatId))
        {
            discountPercent = 5m;
        }
        else
        {
            discountPercent = 0;
        }

        decimal totalCost = wineEventPrice * guestCount;

        if (discountPercent > 0)
        {
            decimal discountAmount = totalCost * (discountPercent / 100m);
            totalCost -= discountAmount;
        }

        return totalCost;
    }
    
    private async Task<bool> IsUserSubscribedAsync(long chatId)
    {
        try
        {
            var chatMember = await _botClient.GetChatMember(AppConfiguration.EventChannelId, chatId);
            return chatMember.Status == ChatMemberStatus.Member;
        }
        catch
        {
            Console.WriteLine($"Ошибка проверки подписки для пользователя {chatId}.");
            return false;
        }
    }

    private async Task<bool> IsRegularGuest(long chatId)
    {
        return await _dbContext.Reservations
            .CountAsync(x => x.Guest!.TelegramId == chatId && x.Status == ReservationStatus.Completed) > 1;
    }
}
