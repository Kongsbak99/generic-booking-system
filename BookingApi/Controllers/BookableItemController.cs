using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BookingApi.Models;
using BookingApi.DTOs;


namespace BookingApi.Controllers
{
    [Route("api/[controller]")] //Base URL for controller - '[controller]' should be the name of controller (i.e. todoitems)
    [ApiController]
    public class BookableItemController : ControllerBase
    {
        private readonly BookableItemContext _context;

        public BookableItemController(BookableItemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookableItem>>> GetBookableItem() {
            if (_context.BookableItems == null) {
                return NotFound();
            }
            return await _context.BookableItems.ToListAsync();
        }


        [HttpPost]
        public async Task<ActionResult<BookableItem>> PostBookableItem(BookableItem bookableItem){
            _context.BookableItems.Add(bookableItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBookableItem), new {id = bookableItem.Id}, bookableItem);
        }

        
    }
}
