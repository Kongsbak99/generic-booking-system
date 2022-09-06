using BookingApi.Database.Models;

namespace BookingApi.Database.Models
{
    public class Booking
    {
        public long Id { get; set; } //The Id property functions as the unique key in a relational database.
        public string? ContactPerson { get; set; }
        public BookableItem? BookedItem { get; set; }

    }
}