using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// 1. Hent tilkoblingsstrengen fra Aspire (sjekker 'mysql' først, deretter 'mariadbcontainer', og fallback)
var connectionString = builder.Configuration.GetConnectionString("mysql")
    ?? builder.Configuration.GetConnectionString("mariadbcontainer")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Missing database connection string.");

// 2. Registrer MySqlConnection slik at nye instanser opprettes riktig i kontrollerne
builder.Services.AddTransient<MySqlConnection>(_ => new MySqlConnection(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();