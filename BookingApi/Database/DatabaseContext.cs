using BookingApi.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Database;

public class DatabaseContext : IdentityDbContext<User, IdentityRole, string> {
    public DatabaseContext(DbContextOptions<DatabaseContext> options) :
        base(options) {
    }

    public DatabaseContext() {
    }

    public DbSet<BookableItem> BookableItems {get; set;}
    public DbSet<Booking> Bookings {get;set;}
    public DbSet<Location> Location {get;set;}

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

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new SeedRoles());
        builder.ApplyConfiguration(new SeedAdminUser());
        builder.ApplyConfiguration(new SeedAdminRole());


        builder.Entity<User>()
            .Ignore(u => u.EmailConfirmed)
            .Ignore(u => u.PhoneNumberConfirmed)
            .Ignore(u => u.TwoFactorEnabled)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.AccessFailedCount);

        builder.Entity<Location>().Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Entity<Location>()
            .HasMany(x => x.BookableItems)
            .WithOne(x => x.Location);

        builder.Entity<BookableItem>().Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Entity<BookableItem>()
            .HasMany(x => x.Bookings)
            .WithOne(x => x.BookableItem);
        builder.Entity<BookableItem>()
            .HasOne(x => x.Location)
            .WithMany(x=>x.BookableItems);

        builder.Entity<Booking>().Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Entity<Booking>()
            .HasOne(x => x.BookableItem)
            .WithMany(x => x.Bookings);

    }
    
    
}
