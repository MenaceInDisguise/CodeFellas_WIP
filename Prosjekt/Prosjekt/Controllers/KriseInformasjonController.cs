using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class KriseInformasjonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
