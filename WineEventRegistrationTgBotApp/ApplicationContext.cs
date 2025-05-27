using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WineEventRegistrationBotApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<WineEvent> WineEvents => Set<WineEvent>();

        public DbSet<Guest> Guests => Set<Guest>();

        public DbSet<Reservation> Reservations => Set<Reservation>();

        public ApplicationContext() { }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            // для автоматических миграций при старте (учебный вариант):
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=tg_bot_app.db");
            }
        }
    }
}
