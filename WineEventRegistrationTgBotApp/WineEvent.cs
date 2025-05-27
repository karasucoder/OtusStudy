using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineEventRegistrationBotApp
{
    public class WineEvent
    {
        public int Id { get; set; }

        public string? Name { get; set;  }

        public string? Description { get; set; }

        public string? Date { get; set; }    

        public string? Time {  get; set; }

        public string? Location { get; set; }

        public decimal Price { get; set; }
    }
}
