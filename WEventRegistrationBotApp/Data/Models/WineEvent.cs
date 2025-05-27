using System.ComponentModel.DataAnnotations;

namespace WEventRegistrationBotApp.Data.Models;

public class WineEvent
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(1000)]
    public string Description { get; set; }

    public DateTime EventDate { get; set; }

    [Required]
    [StringLength(100)]
    public string Location { get; set; }

    [Range(0, 100000)]
    public decimal Price { get; set; }

    public int MaxParticipants { get; set; } = 20;

    public WineEventType EventType { get; set; }

    [StringLength(500)]
    public string? Comment { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}