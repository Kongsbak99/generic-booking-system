using System.ComponentModel.DataAnnotations;

namespace BookingApi.Models.DTOs;

public class LocationDTO : IDTO {
    public string Id {get; set;}
    public string City { get; set; }
    public string Zip { get; set; }
    public string Street { get; set; }
}