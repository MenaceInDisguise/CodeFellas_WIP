using Microsoft.AspNetCore.Mvc;
using Prosjekt.Models.ModelView;

namespace Prosjekt.Controllers
{
    public class RessursController : Controller
    {
        //Viser skjemaet for å registrere en ny ressurs.
        public IActionResult Index()
        {
            return View(new RessursViewModel());
        }
        [HttpPost]
        //Mottar informasjon fra ressursskjemaet og behandler den.
        public ActionResult Create(RessursViewModel model)
        {
            //Kontrollerer at navn, beskrivelse og antall er gyldig.
            if (model.Navn == null || model.Beskrivelse == null || model.Antall <= 0)
            {
                throw new ArgumentException("Navn, beskrivelse eller antall kan ikke være null.");
            }
            //Sender den informasjonen videre til viewet.
            return View(model);
        }

    }
}
