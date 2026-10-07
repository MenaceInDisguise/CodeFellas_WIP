using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class SettingsController : Controller
    {
        //Shows the settings page.
        public IActionResult Index()
        {
            return View();
        }
    }
}
