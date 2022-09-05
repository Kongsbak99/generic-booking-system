using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;


using Api.DTO;


namespace Api.Models
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