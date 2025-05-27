using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace WEventRegistrationBotApp.Utilities
{
    public static class TelegramButtons
    {
        public static InlineKeyboardButton ManagerContactButton =>
            InlineKeyboardButton.WithUrl(
                "Связаться с организатором",
                $"https://t.me/{AppConfiguration.ManagerChatId}");

        public static InlineKeyboardButton ShowEventScheduleButton =>
            InlineKeyboardButton.WithCallbackData(
                "Расписание мероприятий",
                "show_event_schedule");

        public static InlineKeyboardButton EventReservationButton =>
            InlineKeyboardButton.WithCallbackData(
                "Записаться на мероприятие",
                "event_reservation");

        public static InlineKeyboardButton ChangeEventForReservationButton =>
            InlineKeyboardButton.WithCallbackData(
                "Выбрать другое мероприятие",
                "change_event_for_reservation");

        public static InlineKeyboardButton ConfrimReservationButton =>
            InlineKeyboardButton.WithCallbackData(
                "Подтвердить бронь",
                "confirm_reservation");
        
        public static InlineKeyboardButton CancelReservationButton =>
            InlineKeyboardButton.WithCallbackData(
                "Отменить бронь",
                "cancel_reservation");
    }
}
