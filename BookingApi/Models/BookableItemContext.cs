using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;


using BookingApi.DTOs;


namespace BookingApi.Models
{
    public class BookableItemContext : DbContext
    {
        public BookableItemContext(DbContextOptions<BookableItemContext> options)
            : base(options)
        {
        }

        public DbSet<BookableItem> BookableItems { get; set; } = null!;
    }
}