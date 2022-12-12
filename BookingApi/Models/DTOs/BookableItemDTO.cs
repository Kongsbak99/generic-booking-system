using System.ComponentModel.DataAnnotations; 
using BookingApi.Database.Entities;

namespace BookingApi.Models.DTOs;

public class BookableItemDTO : IDTO {
    public string Id {get; set;}
    public string Name { get; set; }
    public string Description { get; set; }
    public LocationDTO Location { get; set; }
    public List<BookingDTO> Bookings { get; set; }
}
public class AddBookableItemDTO {
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string LocationId { get; set; }
}