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
            if (model.Navn == null || model.Beskrivelse == null || model.Antall <= 0)
            {
                throw new ArgumentException("Navn, beskrivelse eller antall kan ikke være null.");
            }
            return View(model);
        }

    }
}
