using BookingApi.Database;
using BookingApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories;

public class BookableItemRepository : GenericRepository<BookableItem, DatabaseContext> {
    private readonly DatabaseContext _context; 

    public BookableItemRepository(DatabaseContext context) : base(context) {
        _context = context;
    }

    public async Task<List<BookableItem>> GetBookableItems(CancellationToken ct) {
        var bookable_items = await _context
            .BookableItems
            .Include(x => x.Location)
            .ToListAsync(ct);
        return bookable_items;
    }


    public async Task<BookableItem> GetSpecificItem(string Id, CancellationToken ct) {
        var bookable_item = await _context
            .BookableItems
            .Where(x => x.Id == Id)
            .Include(x => x.Bookings)
            .Include(x => x.Location)
            .SingleOrDefaultAsync(ct);
        return bookable_item;
    }

    public async Task<bool> BookItem(string bookingId, string BookableItemId,  CancellationToken ct) { 
        var booking = await _context
            .Bookings
            .SingleOrDefaultAsync(x => x.Id == bookingId, ct);
        var bookable_item = await _context.BookableItems.Where(x => x.Id == BookableItemId)
            .Include(x => x.Bookings)
            .SingleOrDefaultAsync(ct);

        if (booking != null && bookable_item != null) {
            bookable_item.Bookings.Add(booking);
            await _context.SaveChangesAsync(ct);
            return true;
        }
        return false;
    }

}