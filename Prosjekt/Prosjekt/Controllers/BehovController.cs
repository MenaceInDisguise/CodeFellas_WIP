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
        public ActionResult Create(BehovViewModel model)
        {   
            if (model.Name == null || model.Description == null || model.Total <= 0)
            {
                throw new ArgumentException("Navn, beskrivelse eller antall kan ikke være null.");
            }
            return View(model);
        }

    }
}
