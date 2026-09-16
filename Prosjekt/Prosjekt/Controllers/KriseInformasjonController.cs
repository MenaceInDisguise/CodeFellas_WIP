using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class KriseInformasjonController : Controller
    {
        //Viser side med informasjon om krisesituasjoner.
        public IActionResult Index()
        {
            return View();
        }
    }
}
