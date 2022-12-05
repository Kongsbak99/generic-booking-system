using System.ComponentModel.DataAnnotations;
using BookingApi.Database.Entities;

namespace BookingApi.Models.DTOs;

public class BookingDTO : IDTO {
    public string Id {get; set;}
    public string Name {get;set;}
    public string PhoneNumber {get; set;}
    public string Email {get; set;}    
    [Required] public string BookableItemId {get;set;}
    [Required] public DateTime From { get; set; }
    [Required] public DateTime To { get; set; }
}