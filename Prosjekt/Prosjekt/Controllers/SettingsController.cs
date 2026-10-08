using Microsoft.AspNetCore.Mvc;
namespace Prosjekt.Controllers
{
    public class SettingsController : Controller
    {
        /// <summary>
        /// Returns the default view for the current controller action.
        /// </summary>
        /// <returns>An IActionResult that renders the default view.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}