using Microsoft.EntityFrameworkCore;
using System;
using System.Text;

using BookingApi.Database.Models;


namespace BookingApi.Database
{
  class Program
  {
    static void Main(string[] args)
    {
      InsertData();
      PrintData();
    }

    private static void InsertData()
    {
      using(var context = new DatabaseContext())
      {
        // Creates the database if not exists
        context.Database.EnsureCreated();

        // Adds a bookable item
        var bookableItem = new BookableItem
        {
          Name = "Table 1",
          Location = "Test Location",
          Description = "This is a description of the bookable item",
        };
        context.BookableItem.Add(bookableItem);

        // Adds some bookings
        context.Booking.Add(new Booking
        {
          ContactPerson = "Victor Kongsbak",
          BookedItem = bookableItem
        });
        context.Booking.Add(new Booking
        {
          ContactPerson = "Test Name",
          BookedItem = bookableItem
        });

        // Saves changes
        context.SaveChanges();
      }
    }

    private static void PrintData()
    {
      // Gets and prints all books in database
      using (var context = new DatabaseContext())
      {
        var bookings = context.Booking
          .Include(p => p.BookedItem);
        foreach(var booking in bookings)
        {
          var data = new StringBuilder();
          data.AppendLine($"ContactPerson: {booking.ContactPerson}");
          data.AppendLine($"BookedItem: {booking.BookedItem.Name}");
          Console.WriteLine(data.ToString());
        }
      }
    }
  }
}



/*
using Microsoft.EntityFrameworkCore;
using BookingApi.Database;
using BookingApi.Database.Models;
using BookingApi.Controllers;
using BookingApi;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(opt =>
    opt.UseInMemoryDatabase("BookingDB"));



//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "BookingApi", Version = "v1" });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookingApi v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
*/