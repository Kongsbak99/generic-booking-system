using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BookingApi.Database.Entities;
using BookingApi.Database;


namespace BookingApi.Controllers
{
    [Route("api/[controller]")] //Base URL for controller - '[controller]' should be the name of controller (i.e. todoitems)
    [ApiController]
    public class BookableItemController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public BookableItemController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookableItem>>> GetBookableItem() {
            if (_context.BookableItem == null) {
                return NotFound();
            }
            return await _context.BookableItem.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<BookableItem>> PostBookableItem(BookableItem bookableItem){
            _context.BookableItem.Add(bookableItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookableItem), new {id = bookableItem.Id}, bookableItem);
        }

        
    }
}
