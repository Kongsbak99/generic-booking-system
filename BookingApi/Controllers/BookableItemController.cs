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
public class BookableItemController : ControllerBase {
    private readonly ILogger<BookableItemController> _logger;
    private readonly IMapper _mapper;
    private readonly BookableItemRepository _bookableItemRepository;
    private readonly BookingRepository _bookingRepository;
    private readonly TokenUtils _tokenUtils;
        
    // Construct
    public BookableItemController(IMapper mapper, BookableItemRepository bookableItemRepository, 
        BookingRepository bookingRepository, TokenUtils tokenUtils, ILogger<BookableItemController> logger) {
        _logger = logger;
        _mapper = mapper;
        _bookableItemRepository = bookableItemRepository;
        _bookingRepository = bookingRepository;
        _tokenUtils = tokenUtils; 
    }

    //XXXXXXXXXXXXXXXXXXXXXXXXXXXX API endpoints XXXXXXXXXXXXXXXXXXXXXXXXXXXX


    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXX  GET  XXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // Get: https://localhost:7206/api/BookableItem 
    [Authorize]
    [HttpGet(Name = "GetBookableItems")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookableItems(CancellationToken ct) {
        var bookable_items = await _bookableItemRepository.GetBookableItems(ct);
        var response = _mapper.Map<IList<BookableItemDTO>>(bookable_items).ToList();


        if (response.Count > 0) {
            return Ok(response);
        }
        return NotFound("Unable to find any bookable items. Database might be empty.");

    }




    // Get: https://localhost:7206/api/BookableItem/{Id}
    [Authorize]
    [HttpGet("{Id}", Name = "GetBookableItem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBookableItem(string Id, CancellationToken ct) {
        var bookable_item = await _bookableItemRepository.GetSpecificItem(Id, ct);


        if (bookable_item != null) {
            var result = _mapper.Map<BookableItemDTO>(bookable_item);
            return Ok(result);
        }
        return NotFound($"No item found with ID: {Id}");
    }





    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXX  POST  XXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // Post: https://localhost:7206/api/BookableItem
    [Authorize(Roles = "Admin")]
    [HttpPost(Name = "AddBookableItem")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddBookableItem([FromBody] AddBookableItemDTO bookableItemDTO, CancellationToken ct) {
        if (ModelState.IsValid) {
            var bookable_item = _mapper.Map<BookableItem>(bookableItemDTO);
            var response = await _bookableItemRepository.Create(bookable_item, ct);
            var result = _mapper.Map<BookableItem>(response);
            return Ok(result);
        }

        _logger.LogInformation($"Invalid POST in {nameof(AddBookableItem)}");
        return BadRequest(ModelState);
    }






    //[Authorize]
    [HttpPost("Book", Name = "BookItem")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
    public async Task<IActionResult> BookItem([FromBody] BookingDTO bookingDTO, CancellationToken ct) {
        //var access_token = await HttpContext.GetTokenAsync("Bearer", "access_token");
        //var userIdFromToken = _tokenUtils.GetUserIdFromToken(access_token);

        if (ModelState.IsValid) {
            var booking = _mapper.Map<Booking>(bookingDTO);
            var response = await _bookingRepository.Create(booking, ct);
            var result = _mapper.Map<Booking>(response);
            var final = await _bookableItemRepository.BookItem(result.Id, result.BookableItemId, ct);
            return final ? NoContent() : NotFound($"No item found with ID: ");
        }
        return BadRequest(ModelState);        
    }


    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    // XXXXXXXXXXXXXXXXXXXXXXXXXXXX  DELETE  XXXXXXXXXXXXXXXXXXXXXXXXXXXX
    //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
    [Authorize]
    [HttpDelete("{Id}", Name = "DeleteItem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteItem(string Id, CancellationToken ct) {
        var bookable_item = await _bookableItemRepository.GetSpecificItem(Id, ct);


        if (bookable_item != null) {
            var result = await _bookableItemRepository.Delete(Id, ct);
            return Ok(result);
        }
        return NotFound($"No item found with ID: {Id}");
    }






}