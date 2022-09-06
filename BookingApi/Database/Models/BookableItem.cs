using BookingApi.Database.Models;

namespace BookingApi.Database.Models
{
    public class BookableItem
    {
        public long Id { get; set; } //The Id property functions as the unique key in a relational database.
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Booking>? Bookings { get; set; }

        public string? Location { get; set; }
        
    }
}