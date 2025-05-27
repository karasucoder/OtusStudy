using Microsoft.EntityFrameworkCore;
using WEventRegistrationBotApp.Data.Models;

namespace WEventRegistrationBotApp.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Guest> Guests => Set<Guest>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

    public DbSet<WineEvent> WineEvents => Set<WineEvent>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(AppConfiguration.DefaultConnection);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp with time zone");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(e => e.ReservationNumber)
                  .UseIdentityAlwaysColumn();
        });
    }
}
