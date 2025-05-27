namespace WEventRegistrationBotApp.Data.Models.State;

public class GuestState
{
    public long ChatId { get; set; }

    public EventReservationStep CurrentStep { get; set; }

    public int SelectedEventId { get; set; }

    public int GuestCount { get; set; }

    public string? GuestName { get; set; }

    public string? GuestPhoneNumber { get; set; }
}