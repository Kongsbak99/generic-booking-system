using BookingApi.Database;
using BookingApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories;

public class BookingRepository : GenericRepository<Booking, DatabaseContext> {
    private readonly DatabaseContext _context; 

    public BookingRepository(DatabaseContext context) : base(context) {
        _context = context;
    }



}