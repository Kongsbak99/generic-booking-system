using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BookingApi.Database.Models;
using BookingApi.Database;


namespace BookingApi.Controllers
{
    [Route("api/[controller]")] //Base URL for controller - '[controller]' should be the name of controller (i.e. todoitems)
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public BookingController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking() {
            if (_context.Booking == null) {
                return NotFound();
            }
            return await _context.Booking.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<Booking>> PostBookableItem(Booking booking){
            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBooking), new {id = booking.Id}, booking);
        }

        
    }
}
