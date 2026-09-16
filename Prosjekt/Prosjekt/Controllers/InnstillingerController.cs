using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class InnstillingerController : Controller
    {
        //Viser siden for innstillinger.
        public IActionResult Index()
        {
            return View();
        }
    }
}
