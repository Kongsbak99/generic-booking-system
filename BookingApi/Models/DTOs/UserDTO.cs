using System.ComponentModel.DataAnnotations;

namespace BookingApi.Models.DTOs;

public class UserDTO : IDTO {
    [Required] public string Id { get; set; }
    [Required] public string Firstname { get; set; }
    [Required] public string Lastname { get; set; }
    [DataType(DataType.Date)] public DateTime Birthdate { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string City { get; set; }
    [Required] public string Zip { get; set; }
    [Required] public string Street { get; set; }
    [Required] public string PhoneNumber { get; set; }
}
public class CreateUserDTO {
    [Required] public string Firstname { get; set; }
    [Required] public string Lastname { get; set; }
    [DataType(DataType.Date)] public DateTime Birthdate { get; set; } 
    public ICollection<string> Roles { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string City { get; set; }
    [Required] public string Zip { get; set; }
    [Required] public string Street { get; set; }
    [Required] public string PhoneNumber { get; set; }
}
public class LoginDTO {
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
