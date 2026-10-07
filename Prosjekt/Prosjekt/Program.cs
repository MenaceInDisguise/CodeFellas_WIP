using Microsoft.EntityFrameworkCore;
using Prosjekt.DataAccess;
using Prosjekt.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// 1. Get the connection string for kartdb from Aspire or appsettings.json
var connectionString = builder.Configuration.GetConnectionString("kartdb")
    ?? builder.Configuration.GetConnectionString("mariadbcontainer")
    ?? builder.Configuration.GetConnectionString("mysql")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing database connection string for 'kartdb'.");

// 2. Register repository
builder.Services.AddScoped<IGeoChangeRepository, GeoChangeRepository>();

// 3. Register DbContext with a fixed MariaDB version and built-in transient error handling
var serverVersion = new MariaDbServerVersion(new Version(11, 4, 0));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString,
        serverVersion,
        mySqlOptions => mySqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
    )
);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4. Direct database initialization without a wait loop
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var db = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        db.Database.Migrate();
        logger.LogInformation("Database 'kartdb' migrated and initialized!");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Migrate failed, attempting EnsureCreated as fallback...");
        try
        {
            db.Database.EnsureCreated();
            logger.LogInformation("Database 'kartdb' ensured with EnsureCreated.");
        }
        catch (Exception finalEx)
        {
            logger.LogError(finalEx, "Failed to initialize the database 'kartdb'.");
            throw;
        }
    }
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();


