using BookingApi.Configuration;
using BookingApi.Database;
using BookingApi.Utils;
using BookingApi.Repositories;
using BookingApi.Services;

using AspNetCoreRateLimit;

using Microsoft.AspNetCore.Mvc;

using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => { options.EnableSensitiveDataLogging(); });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<BookableItemRepository>();
builder.Services.AddScoped<LocationRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<TokenUtils>();

builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimit();
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureCors();
builder.Services.ConfigureHttpCacheHeaders();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);

builder.Services.AddAutoMapper(typeof(ObjectMapper));

var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .Enrich
    .FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services
    .AddControllers(config => {
        config
            .CacheProfiles
            .Add("duration120sec", new CacheProfile {Duration = 120});
    })
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

using (var client = new DatabaseContext())
    {
        client.Database.EnsureCreated();
    }


var app = builder.Build();

app.ConfigureExceptionHandler();
app.UseCors("CorsPolicyAllowAll");
app.UseResponseCaching();
app.UseHttpCacheHeaders();
app.UseIpRateLimiting();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


try {
    logger.Information("\n");
    logger.Information("Starting api");
    app.Run();
}
catch (Exception e) {
    logger.Fatal(e, "Failed on startup");
}
finally {
    logger.Information("Exiting");
    logger.Dispose();
}