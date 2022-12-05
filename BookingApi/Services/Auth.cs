using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingApi.Database.Entities;
using BookingApi.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BookingApi.Services;

public class Auth : IAuth {
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User _user;

    public Auth(UserManager<User> userManager, IConfiguration configuration) {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> Validate(LoginUserDTO loginUserDto) {
        _user = await _userManager.FindByNameAsync(loginUserDto.Email);
        return (_user != null && await _userManager.CheckPasswordAsync(_user, loginUserDto.Password));
    }

    public async Task<string> GenerateToken() {
        var signingCredentials = FetchCredentials();
        var claims = await FetchClaims(); // specific user claims
        var token = GenerateToken(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken GenerateToken(SigningCredentials credentials, List<Claim> c) {
        var jwtSettings = _configuration.GetSection("JwtToken");

        var expiration = DateTime
            .Now
            .AddDays(14);

        var issuer = jwtSettings.GetSection("Issuer").Value;

        var token = new JwtSecurityToken(
            issuer: issuer,
            claims: c,
            expires: expiration,
            signingCredentials: credentials
        );

        return token;
    }

    private async Task<List<Claim>> FetchClaims() {
        var claims = new List<Claim> {
            new(ClaimTypes.Name, _user.UserName),
            new("id", _user.Id)
        };

        var roles = await _userManager.GetRolesAsync(_user);

        claims.AddRange(roles
            .Select(role => new Claim(ClaimTypes.Role, role)));
        return claims;
    }

    private SigningCredentials FetchCredentials() {
        var jwtSettings = _configuration.GetSection("JwtToken");
        var key = jwtSettings.GetSection("Key").Value;
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
}