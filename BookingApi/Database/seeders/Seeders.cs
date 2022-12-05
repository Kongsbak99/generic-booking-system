using BookingApi.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Database;

public class Seeders {
}

//Seed user identity roles
public class SeedRoles : IEntityTypeConfiguration<IdentityRole> {
    public void Configure(EntityTypeBuilder<IdentityRole> b) {
        b.HasData(
            new IdentityRole {
                Id = "-2",
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole {
                Id = "-1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            }
        );
    }
}

//Seed admin role
public class SeedAdminRole : IEntityTypeConfiguration<IdentityUserRole<string>> {
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> b) {
        var assignRole = new IdentityUserRole<string> {
            RoleId = "-1",
            UserId = "-1"
        };
        b.HasData(assignRole);
    }
}

//Seed admin USER 
//This way an admin user will always exists
public class SeedAdminUser : IEntityTypeConfiguration<User> {
    private string GeneratePassword(User user) {
        var passHash = new PasswordHasher<User>();
        return passHash.HashPassword(user, "admin");
    }
    public void Configure(EntityTypeBuilder<User> b) {
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
        admin.PasswordHash = GeneratePassword(admin);
        b.HasData(admin);
    }
}