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

    [HttpGet]
    public IActionResult Index()
    {
        var posisjoner = Prosjekt.Controllers.GeoEndringController.GetRegisteredPositions();
        return View(posisjoner);
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
