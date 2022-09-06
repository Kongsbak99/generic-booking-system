using BookingApi.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Database;

public class Seeders {
}

public class SeedBookableItems : IEntityTypeConfiguration<BookableItem>{
    public void Configure(EntityTypeBuilder<BookableItem> builder) {
        builder.HasData(
            new BookableItem {
                Id = -1,
                Name = "",
                Location = "",
                Description = ""
            }
        );
    }
}
public class SeedBookings : IEntityTypeConfiguration<Booking>{
    public void Configure(EntityTypeBuilder<Booking> builder) {
        builder.HasData(
            new Booking {
                Id = -1,
                ContactPerson = "",
                BookableItemId = 0
            }
        );
    }
}
/*
public class SeedAdminRole : IEntityTypeConfiguration<IdentityUserRole<string>> {
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder) {
        var assignRole = new IdentityUserRole<string> {
            RoleId = "-1",
            UserId = "-1"
        };

        builder.HasData(assignRole);
    }
}

public class SeedAdminUser : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        var admin = new User {
            Id = "-1",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Firstname = "ADMIN",
            Lastname = "ADMIN",
            NormalizedEmail = "ADMIN",
            Birthdate = DateTime.Now,
            Email = "ADMIN",
            City = "ADMIN",
            Zip = "ADMIN",
            Street = "ADMIN",
            PhoneNumber = "ADMIN",
        };
        admin.PasswordHash = PwGenerator(admin);
        builder.HasData(admin);
    }

    private string PwGenerator(User user) {
        var passHash = new PasswordHasher<User>();
        return passHash.HashPassword(user, "admin");
    }
}*/