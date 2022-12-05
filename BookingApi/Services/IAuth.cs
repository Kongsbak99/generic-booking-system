using BookingApi.Models.DTOs;

namespace BookingApi.Services;

public interface IAuth {
    Task<bool> Validate(LoginUserDTO loginUserDto);
    Task<string> GenerateToken();
}