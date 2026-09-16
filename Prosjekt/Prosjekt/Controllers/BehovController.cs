using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class BehovController : Controller
    {
        public IActionResult Index()
        {
            return View(new BehovViewModel());
        }
        [HttpPost]
        public IActionResult Create(BehovViewModel model)
        {
           
            if (string.IsNullOrWhiteSpace(model.Navn) || string.IsNullOrWhiteSpace(model.Beskrivelse) || model.Totalt <= 0)
            {
                ModelState.AddModelError("", "Alle felt må fylles ut gyldig.");
                return View("Index", model);
            }

            return View(model);
        }
    }
}
