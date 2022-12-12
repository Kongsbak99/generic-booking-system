using BookingApi.Models.DTOs;

namespace BookingApi.Services;

public interface IAuth {
    Task<bool> Validate(LoginDTO dto);
    Task<string> GenerateToken();
}