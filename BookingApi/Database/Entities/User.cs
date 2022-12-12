using Microsoft.AspNetCore.Identity;

namespace BookingApi.Database.Entities;

public class User : IdentityUser, IEntity {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string Street { get; set; }
    public DateTime Birthdate { get; set; }

}