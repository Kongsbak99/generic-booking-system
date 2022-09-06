using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore;

using BookingApi.Database.Models;

namespace BookingApi.Database
{
  public class DatabaseContext : DbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) :
        base(options) {
    }
    public DatabaseContext() {
    }
    public DbSet<BookableItem> BookableItem { get; set; }

    public DbSet<Booking> Booking { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (optionsBuilder.IsConfigured) return;

        const string appSettings = "appsettings.json";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(appSettings)
            .Build();

        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }


    //Build database
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<BookableItem>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired();
        entity.Property(e => e.Location);
        entity.Property(e => e.Description).IsRequired();
        
      });

      modelBuilder.Entity<Booking>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.ContactPerson).IsRequired();
        entity.HasOne(d => d.BookedItem)
          .WithMany(p => p.Bookings);
      });


    }
  }
}