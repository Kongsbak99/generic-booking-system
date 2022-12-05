namespace BookingApi.Database.Entities; 

public class BookableItem : IEntity {
    public string Id {get; set;}
    public string Name {get; set;}
    public string Description { get; set; }
    public string LocationId {get;set;}
    public Location Location { get; set; }
    public List<Booking> Bookings { get; set; }
}