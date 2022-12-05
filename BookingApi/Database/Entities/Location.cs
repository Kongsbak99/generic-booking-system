
namespace BookingApi.Database.Entities;

public class Location : IEntity {
    public string Id {get; set;}

    public string City { get; set; }

    public string Zip { get; set; }

    public string Street { get; set; }
    public List<BookableItem> BookableItems {get;set;}
}