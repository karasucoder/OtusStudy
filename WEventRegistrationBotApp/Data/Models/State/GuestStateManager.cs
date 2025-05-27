namespace WEventRegistrationBotApp.Data.Models.State;

public class GuestStateManager
{
    private readonly Dictionary<long, GuestState> _guestStates = new();

    public GuestState GetOrCreateGuestState(long chatId)
    {
        if (_guestStates.TryGetValue(chatId, out var state))
        {
            return state;
        }

        state = new GuestState
        {
            ChatId = chatId
        };

        _guestStates[chatId] = state;

        return state;
    }

    public void ResetGuestState(long chatId)
    {
        _guestStates.Remove(chatId);
    }
}
