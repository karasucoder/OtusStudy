using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEventRegistrationBotApp.Data.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(WineEventId))]
        public WineEvent? WineEvent { get; set; }

        public int WineEventId { get; set; }

        [ForeignKey(nameof(MainGuestId))]
        public Guest? Guest { get; set; }

        public int MainGuestId { get; set; }

        public int GuestCount { get; set; }

        public decimal Price { get; set; }

        public decimal Sum { get; set; }

        public ReservationSource Source { get; set; }

        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    }
}
