using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class CrisisInformation : Controller
    {
        /// <summary>
        /// Returns the default view for the controller.
        /// </summary>
        /// <returns>An IActionResult that renders the default view.</returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
