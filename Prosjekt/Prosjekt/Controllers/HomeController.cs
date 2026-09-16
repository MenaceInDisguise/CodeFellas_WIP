using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers;
public class HomeController : Controller
{
    ///Viser hovedsiden til applikasjonen.
    public IActionResult Index()
    {
        return View();
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
