using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddDbContext<TodoContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext"))
    )
    .AddEndpointsApiExplorer()
    .AddControllers();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TodoContext>();

    const int retries = 15;
    const int delaySeconds = 5;
    int attempt = 0;
    bool migrated = false;

    while (attempt < retries && !migrated)
    {
        try
        {
            db.Database.Migrate();
            migrated = true;
            Console.WriteLine("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            attempt++;
            Console.WriteLine($"Attempt {attempt}/{retries}: Failed to connect to the database. Waiting {delaySeconds} seconds...");
            Console.WriteLine($"Error: {ex.Message}");

            if (attempt == retries)
            {
                Console.WriteLine("Failed to connect to the database after multiple attempts.");
                throw;
            }
            Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
        }
    }
}

app.UseAuthorization();
app.MapControllers();
app.Run();
