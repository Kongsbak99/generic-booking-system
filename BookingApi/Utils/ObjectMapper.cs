using AutoMapper;
using BookingApi.Database.Entities;
using BookingApi.Models.DTOs;

namespace BookingApi.Utils;

public class ObjectMapper : Profile {
    public ObjectMapper() {
        CreateMap<Location, LocationDTO>().ReverseMap();

        CreateMap<BookableItem, BookableItemDTO>().ReverseMap();
        CreateMap<BookableItem, AddBookableItemDTO>().ReverseMap();

        CreateMap<BookingDTO, Booking>().ReverseMap();

        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<User, CreateUserDTO>().ReverseMap();
        CreateMap<User, LoginDTO>().ReverseMap();
    }
}


