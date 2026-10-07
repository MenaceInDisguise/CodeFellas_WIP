using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;
using MySqlConnector;

namespace Prosjekt.Controllers;
public class HomeController : Controller
{
    private readonly string _connectionString;

    // Constructor that fetches the connection from Aspire's configuration
    public HomeController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("mysql")
            ?? configuration.GetConnectionString("mariadbcontainer")
            ?? "server=mariadbcontainer;port=3306;database=mysql;user=root;password=Gruppe12!";
    }

    [HttpGet]
    public IActionResult Index()
    {
        var positions = Prosjekt.Controllers.GeoChangeController.GetRegisteredPositions();
        return View(positions);
    }

    //Shows a page with information about privacy.
    public IActionResult PrivacyPolicy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //Shows an error page if an error occurs.
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
