using AutoMapper;
using BookingApi.Database.Entities;
using BookingApi.Models.DTOs;
using BookingApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly IAuth _auth;

    // Construct
    public AuthController(
        UserManager<User> userManager,
        ILogger<AuthController> logger,
        IMapper mapper, IAuth auth) {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _auth = auth;
    }

    [HttpPost("login", Name = "Login")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto) {
        _logger.LogInformation($"Init login attempt: {dto.Email}");
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        if (await _auth.Validate(dto)) {
            return Accepted(new {Token = await _auth.GenerateToken()});
        }

        return Unauthorized();
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateUserDTO userDto) {
        _logger.LogInformation($"Init registration attempt: {userDto.Email}");

        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var user = _mapper.Map<User>(userDto);
        user.UserName = userDto.Email.Split("@")[0];

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded) {
            var role = userDto.Roles;
            await _userManager.AddToRolesAsync(user, role);
            return Accepted();
        }

        foreach (var error in result.Errors) {
            ModelState.AddModelError(error.Code, error.Description);
            _logger.LogInformation($"Error code: {error.Code}");
            _logger.LogInformation($"Error description: {error.Description}");
        }

        return BadRequest(ModelState);
    }
}