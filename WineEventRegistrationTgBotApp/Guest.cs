using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineEventRegistrationBotApp
{
    public class Guest
    {
        public int Id { get; set; }

        public string? TelegramId { get; set; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }
    }
}