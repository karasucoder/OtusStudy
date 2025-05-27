using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineEventRegistrationBotApp
{
    public class ReservationState
    {
        public string SelectedEvent { get; set; }
        public int GuestNumber { get; set; }
        public string MainGuestId { get; set; }
    }
}
