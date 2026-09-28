using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;
using MySqlConnector;

namespace Prosjekt.Controllers;
public class HomeController : Controller
{
    private readonly string _connectionString;

    // Konstruktør som henter tilkoblingen fra Aspire sin konfigurasjon
    public HomeController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("mysql")
            ?? configuration.GetConnectionString("mariadbcontainer")
            ?? "server=mariadbcontainer;port=3306;database=mysql;user=root;password=Gruppe12!";
    }

    public async Task<IActionResult> Index()
    {
        string viewModel1 = "Connected to MariaDB successfully!";
        string viewModel2 = "Failed to connect to MariaDB";

        try
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            return View("Index", viewModel1);
        }
        catch (Exception ex)
        {
            return View("Index", $"{viewModel2}: {ex.Message}");
        }
    }

    //Viser side med informasjon om personvern.
    public IActionResult Personvern()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //Viser en feilmeldingsside dersom det oppstår en feil.
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
