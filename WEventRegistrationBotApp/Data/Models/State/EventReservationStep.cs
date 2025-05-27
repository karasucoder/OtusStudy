using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEventRegistrationBotApp.Data.Models.State
{
    public enum EventReservationStep
    {
        None,
        SelectingEvent,
        EnteringGuestCount,
        EnteringGuestPhoneNumber,
        EnteringGuestName,
        ConfirmingReservation
    }
}
