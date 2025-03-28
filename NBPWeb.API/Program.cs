using Microsoft.AspNetCore.RateLimiting;

namespace NBPWeb.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRateLimiter(options =>
        {
            // Rate limiter policy
            options.AddFixedWindowLimiter("FixedWindowPolicy", opt =>
            {
                opt.Window = TimeSpan.FromMinutes(1); // Time window: 1 minute
                opt.PermitLimit = 100;                // Max requests per window
                opt.QueueLimit = 10;                  // Max queued requests (optional)
            });

            // Custom response when limit is exceeded
            options.OnRejected = (context, _) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return new ValueTask();
            };
        });

        // Add services to the container.
        builder.Services.AddHttpClient("NbpApi", client =>
        {
            client.BaseAddress = new Uri("https://api.nbp.pl/api/exchangerates/");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddMemoryCache();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRateLimiter();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
