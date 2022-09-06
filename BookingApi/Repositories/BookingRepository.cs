using Microsoft.EntityFrameworkCore;
using BookingApi.Database;
using BookingApi.Database.Entities;


namespace BookingApi.Repositories;

public class BookingRepository {
    private readonly DatabaseContext _dbContext;

    /*public BookingRepository(DatabaseContext context) : base(context) {
        _dbContext = context;
    }*/


    public async Task<bool> PostBooking(long BookingId, long BookableItemId) {
        var bookableItem = await _dbContext
            .BookableItem
            .SingleOrDefaultAsync(x => x.Id == BookableItemId);

        var booking = await _dbContext
            .Booking
            .Where(x => x.Id == BookingId)
            .Include(x => x.BookedItem)
            .SingleOrDefaultAsync();

        if (bookableItem != null && booking != null) {
            bookableItem.Bookings.Add(booking);
            //booking.BookedItem = bookableItem;
            //_dbContext.Booking.Add(booking);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

}