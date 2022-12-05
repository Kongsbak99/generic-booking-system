namespace BookingApi.Database.Entities;

public class Booking : IEntity {
    public string Id {get; set;}
    public string Name {get;set;}
    public String PhoneNumber {get; set;}
    public String Email {get; set;}
    public string BookableItemId {get;set;}
    public BookableItem BookableItem {get;set;}
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    
}