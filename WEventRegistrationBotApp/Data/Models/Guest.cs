using System.ComponentModel.DataAnnotations;

namespace WEventRegistrationBotApp.Data.Models;

public class Guest
{
    public int GuestId { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public long? TelegramId { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(20)]
    public string PhoneNumber { get; set; }

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
