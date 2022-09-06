using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BookingApi.Database.Entities;
using BookingApi.Database;
using BookingApi.Repositories;


namespace BookingApi.Controllers
{
    [Route("api/[controller]")] //Base URL for controller - '[controller]' should be the name of controller (i.e. todoitems)
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly BookingRepository _bookingRepository;

        public BookingController(DatabaseContext context){
            _context = context;
        }

        // xxxxxxxxx  API CONTRUCTS   xxxxxxxxxxxxxxxxxxxxx

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking() {
            if (_context.Booking == null) {
                return NotFound();
            }
            return await _context.Booking.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking){
            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new {id = booking.Id}, booking);

            //var response = await _bookingRepository.PostBooking(booking.Id, booking.BookableItemId);
            //return response ? NoContent() : NotFound($"practise with id: {booking.Id} not found");
        }

        
    }
}
