using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class RessursController : Controller
    {
        public IActionResult Index()
        {
            return View(new RessursViewModel());
        }
        [HttpPost]
        public ActionResult Create(RessursViewModel model)
        {   
            return View(model);
        }

    }
}
