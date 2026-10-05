using Microsoft.EntityFrameworkCore;
using Prosjekt.DataAccess;
using Prosjekt.DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// 1. Hent tilkoblingsstreng for kartdb fra Aspire eller appsettings.json
var connectionString = builder.Configuration.GetConnectionString("kartdb")
    ?? builder.Configuration.GetConnectionString("mariadbcontainer")
    ?? builder.Configuration.GetConnectionString("mysql")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing database connection string for 'kartdb'.");

// 2. Registrer repository
builder.Services.AddScoped<IGeoEndringRepository, GeoEndringRepository>();

// 3. Registrer DbContext med fast MariaDB-versjon og innebygd transient feilhåndtering
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

// 4. Direkte databaseinitialisering uten venteløkke
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var db = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        db.Database.Migrate();
        logger.LogInformation("Database 'kartdb' migrert og initialisert!");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Migrate feilet, forsøker EnsureCreated som fallback...");
        try
        {
            db.Database.EnsureCreated();
            logger.LogInformation("Database 'kartdb' sikret med EnsureCreated.");
        }
        catch (Exception finalEx)
        {
            logger.LogError(finalEx, "Klarte ikke å initialisere databasen 'kartdb'.");
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


