using Microsoft.AspNetCore.Mvc;

namespace Prosjekt.Controllers
{
    public class CrisisInformationController : Controller
    {
        //Shows a page with information about crisis situations.
        public IActionResult Index()
        {
            return View();
        }
    }
}
