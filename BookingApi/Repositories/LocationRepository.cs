using BookingApi.Database;
using BookingApi.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Repositories;

public class LocationRepository : GenericRepository<Location, DatabaseContext> {
    private readonly DatabaseContext _context; 

    public LocationRepository(DatabaseContext context) : base(context) {
        _context = context;
    }


    public async Task<Location> GetSpecificItem(string Id, CancellationToken ct) {
        var location = await _context
            .Location
            .Where(x => x.Id == Id)
            .SingleOrDefaultAsync(ct);
        return location;
    }

    


}