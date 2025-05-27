using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineEventRegistrationBotApp
{
    public class Reservation
    {
        public int Id { get; set; }

        public int ReservationNumber { get; set; }

        public DateTime DateTime { get; set; }

        // навигационное свойство для получения WineEvent
        public WineEvent? WineEvent { get; set; }

        public int WineEventId { get; set; }

        // навигационное свойство для получения Guest
        public Guest? Guest { get; set; }

        public int MainGuestId { get; set; }
    }
}
