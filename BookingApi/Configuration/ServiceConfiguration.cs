using System.Text;
using AspNetCoreRateLimit;
using BookingApi.Database;
using BookingApi.Database.Entities;
using BookingApi.Models;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace BookingApi.Configuration;

public static class ServiceConfiguration {
    // Allow all port otherwise blocked by CORS
    public static void ConfigureCors(this IServiceCollection s) {
        s.AddCors(o => {
            o.AddPolicy("CorsPolicyAllowAll",
                policy => {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
    }

    // Configure limits 
    public static void ConfigureRateLimit(this IServiceCollection s) {
        var ruleList = new List<RateLimitRule> {
            new RateLimitRule {
                Endpoint = "*",
                Period = "0.5s",
                Limit = 20
            }
        };
        s.Configure<IpRateLimitOptions>(o => { o.GeneralRules = ruleList; });
        s.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        s.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        s.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        s.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }

    //COnfigure Json web token auth
    public static void ConfigureJwt(this IServiceCollection s, IConfiguration c) {
        var jwt = c
            .GetSection("JwtToken");
        var key = jwt.GetSection("Key").Value; //if in a real world app, the key shouldnt be set in appsettings
        s.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwt.GetSection("Issuer").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateAudience = false
            };
        });
    }

    // Entity Framework identity configuration
    public static void ConfigureIdentity(this IServiceCollection s) {
        var b = s
            .AddIdentityCore<User>(options => { options.User.RequireUniqueEmail = true; });

        b = new IdentityBuilder(b.UserType, typeof(IdentityRole), s);
        b.AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
    }

    public static void ConfigureHttpCacheHeaders(this IServiceCollection s) {
        s.AddResponseCaching();
        s.AddHttpCacheHeaders(
            expirationOptions => {
                expirationOptions.MaxAge = 180;
                expirationOptions.CacheLocation = CacheLocation.Private;
            },
            (validationOptions) => { validationOptions.MustRevalidate = true; }
        );
    }

    public static void ConfigureExceptionHandler(this IApplicationBuilder app) {
        app.UseExceptionHandler(error => {
            error.Run(async context => {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null) {
                    Log.Error($"Error in {contextFeature.Error}");
                    await context.Response.WriteAsync(new Error {
                        StatusCode = context.Response.StatusCode,
                        Message = $"Internal server error. Please contact the issuer of this api, if the problem persist." 
                    }.ToString());
                }
            });
        });
    }

    public static void ConfigureApiVersioning(this IServiceCollection s) {
        s.AddApiVersioning(o => {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ApiVersionReader = new HeaderApiVersionReader("api-version");
        });
    }
}