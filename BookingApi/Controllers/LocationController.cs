using AutoMapper;
using BookingApi.Database.Entities;
using BookingApi.Models.DTOs;
using BookingApi.Repositories;
using BookingApi.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Controllers;

[ApiVersion("1.0", Deprecated = false)]
[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase {
    private readonly ILogger<LocationController> _logger;
    private readonly IMapper _mapper;
    private readonly LocationRepository _locationRepository;

    private readonly TokenUtils _tokenUtils;

    // Construct
    public LocationController(IMapper mapper, LocationRepository locationRepository, TokenUtils tokenUtils,
        ILogger<LocationController> logger) {
        _logger = logger;
        _mapper = mapper;
        _locationRepository = locationRepository;
        _tokenUtils = tokenUtils; 
    }



    [Authorize(Roles = "Admin")]
    [HttpPost(Name = "AddLocation")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddLocation([FromBody] LocationDTO locationDTO, CancellationToken ct) {
        if (ModelState.IsValid) {
            var location = _mapper.Map<Location>(locationDTO);
            var response = await _locationRepository.Create(location, ct);
            var result = _mapper.Map<LocationDTO>(response);
            return Ok(result);
        }

        _logger.LogInformation($"Invalid POST in {nameof(AddLocation)}");
        return BadRequest(ModelState);
    }


    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXX  DELETE  XXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    [Authorize]
    [HttpDelete("{Id}", Name = "DeleteLocation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteItem(string Id, CancellationToken ct) {
        var location = await _locationRepository.GetSpecificItem(Id, ct);


        if (location != null) {
            var result = await _locationRepository.Delete(Id, ct);
            return Ok(result);
        }
        return NotFound($"No item found with ID: {Id}");
    }


}